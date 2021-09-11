

$(function () {



    let checkQuestUrl = '/Home/CheckQuest';
    let startQuestQuestUrl = '/Home/StartQuest';
    let createQuestQuestUrl = '/Home/CreateQuest';

    let countdown = 0;
    let historyTestId = -1;
    let confirmUrl = '/Home/ConfirmTest';
    let isContinute = false;



    $("#aptitutest_form_submit").on("click", function () {
        $('#modal-previewConfirmSubmitForm').modal('show');
    });

    $('#modal-previewConfirmSubmitForm:contains("Do you want to submit")').off('click').on('click', function (e) {
        $('#modal-previewConfirmSubmitForm').modal('hide');
        submitForm();
    });

    function submitForm(timeout = false) {
        clearInterval(countdown);

        $(".count-down-percent").css("animation-play-state", "paused");

        resultQuest = [];

        $.each($(".question"), function (i, q) {
            let QuestionId = $(this).find(".questId").val();

            let Result = "";

            let Answer = $(this).find(".awnsers");
            $.each(Answer, function () {
                if ($(this).find(".corect").is(":checked")) {
                    Result += $(this).find(".corect").val() + ",";
                }
            })

            resultQuest.push({
                "id": i,
                "QuestionId": QuestionId,
                "Result": Result.slice(0, -1)
            });

        });
        ajax('{ resultQuest : ' + JSON.stringify(resultQuest) +
            `, historyTestId: "` + historyTestId + `"}`, confirmUrl, function (data) {

                switch (data.Status) {
                    default:
                        var timespent = Math.floor(data.TotalTime / 60) + ":" + (data.TotalTime - (Math.floor(data.TotalTime / 60) * 60));
                        if (timeout != false) {

                            let timeoutAlert = `<h5 class="text-lg text-theme-6 font-medium leading-none mt-3">` + data.categoryName + ` is timeout</h5>
                                    <div class="box p-5 mt-5">
                                    <div class="flex">
                                        <div class="mr-auto">Spent</div>
                                        <div>`+ timespent +  `</div>
                                    </div>
                                    <div class="flex mt-4">
                                        <div class="mr-auto">Corect mark</div>
                                        <div class="text-theme-6">`+ data.CorectMark + `/` + data.TotalMark + `</div>
                                    </div>
                                    <div class="flex mt-4">
                                        <div class="mr-auto">Your score</div>
                                        <div> `+ data.PercentMark + `%</div>
                                    </div>
                                </div>`;
                            showAlertModal(timeoutAlert, "success");

                        } else {
                            let successAlert = `<h5 class="text-lg text-theme-6 font-medium leading-none mt-3">` + data.categoryName + ` was submitted</h5>
                                    <div class="box p-5 mt-5">
                                    <div class="flex">
                                        <div class="mr-auto">Spent</div>
                                        <div>`+ timespent + `</div>
                                    </div>
                                    <div class="flex mt-4">
                                        <div class="mr-auto">Corect mark</div>
                                        <div class="text-theme-6">`+ data.CorectMark + `/` + data.TotalMark + `</div>
                                    </div>
                                    <div class="flex mt-4">
                                        <div class="mr-auto">Your score</div>
                                        <div> `+ data.PercentMark + `%</div>
                                    </div>
                                </div>`;
                            showAlertModal(successAlert, "success");
                        }
                        $('#modal-previewAlert-success').off('click').on('click', function (e) {
                            $('#modal-previewAlert-success').modal('hide');
                            createQuest($('#candidate-codeId').text());
                        });

                        break;
                }
            });
    }
    createQuest($('#candidate-codeId').text());

    //create quest
    function createQuest(CandidateId) {
        ajax(`{ CandidateId: ` + CandidateId + `}`, checkQuestUrl, function (res) {

            let endDate = new Date(parseInt(res.testScheduleEndValue.replace('/Date(', '')));
            let startDate = new Date(parseInt(res.testScheduleStartValue.replace('/Date(', '')));

            //return false;
            switch (res.Status) {
                case "NotInSchedule":
                    showAlertModal('You are not in the exam schedule <br/> Call the examiner', "not-in-schedule");

                    $('#modal-previewAlert-not-in-schedule').off('click').on('click', function (e) {
                        window.location = '/logout';
                    });

                    setTimeout(function () {
                        $('#modal-previewAlert').modal('hide');
                        window.location = '/logout';
                    }, 30000);
                    break;
                case "NotStartSchedule":
                    showAlertModal(`Your schedule exam hasn't started yet  <br/> Begin in:` + startDate + `<br/>` + `End in: ` + endDate, "not-start-schedule");

                    $('#modal-previewAlert-not-start-schedule').off('click').on('click', function (e) {
                        window.location = '/logout';
                    });

                    setTimeout(function () {
                        $('#modal-previewAlert').modal('hide');
                        window.location = '/logout';
                    }, 30000);
                    break;
                case "EndSchedule":
                    let htmlEndSchedule = `<h3 class="font-medium  text-2xl text-center mb-2 text-theme-6">Result</h3>  <div class="overflow-x-auto">
                             <table class="table">
                                 <thead>
                                     <tr>
                                         <th class="border-b-2 whitespace-no-wrap">Part</th>
                                         <th class="border-b-2 whitespace-no-wrap">Correct</th>
                                         <th class="border-b-2 whitespace-no-wrap">Mark</th>
                                         <th class="border-b-2 whitespace-no-wrap "colspan="2">Time Spent</th>
                                     </tr>
                                 </thead>
                                 <tbody>`;
                    let totalMark = 0;
                    $.each(res.data, function (i, d) {
                        let timespent = "";
                        if ((d.TotalTime - (Math.floor(d.TotalTime / 60) * 60) - 1) <= 0) {
                            timespent = "expired";

                        } else {
                            timespent = Math.floor(d.TotalTime / 60) + ":" + (d.TotalTime - (Math.floor(d.TotalTime / 60) * 60) - 1) + "s";
                        }
                        totalMark += d.PercentMark;

                        htmlEndSchedule += `<tr class="">
                                <td class="border-b">` + d.CategoryExam + `</td>
                                <td class="border-b">` + d.CorectMark + `/` + d.TotalMark + `</td>
                                <td class="border-b">` + d.PercentMark + `% </td>
                                <td class="border-b" >` + timespent + `</td>
                                <td class="border-b">` + d.DateStartTest + `<br/>to ` + d.DateEndTest + `</td>
                                 </tr>`;
                    });
                    let averageMark = (totalMark / 3).toFixed(2);
                    htmlEndSchedule += `<td class="border-b" colspan="2">Grade Point Average: </td>
                            <td class="border-b">` + averageMark + `%</td>
                            <td class="border-b" colspan="2"><strong> Pass if your GPA > 80% </strong></td>`;
                    htmlEndSchedule += `</tbody></table></div>`;

                    if (averageMark > 80) {
                        htmlEndSchedule += `<h3 class="font-medium  text-lg  text-center mb-2 mt-2 text-theme-6">You have cleared this round, next round would be HR Round</h3> `;
                    } else {
                        htmlEndSchedule += `<h3 class="font-medium  text-lg  text-center mb-2 mt-2 text-theme-6">Unfortunate,You have not cleared this round</h3> `;
                    }

                    showAlertModal(`<center><strong>Your schedule exam time is up </strong>  <br/> Begin in:` + startDate + `<br/>` + `End in: ` + endDate + `</center>` + htmlEndSchedule, "end-schedule");

                    $('#modal-previewAlert-end-schedule').off('click').on('click', function (e) {
                        window.location = '/logout';
                    });
                    break;
                case "Beginer":
                    $('#modal-previewConfirmPolicy .btn-CancelModalPolicy').html(`<a href="/logout">Do it later</a>`);
                    $('#modal-previewConfirmPolicy .text-modal').prepend(`
                                                <h2 class="font-semibold">Schedule from ` + res.testScheduleStart + ` to ` + res.testScheduleEnd + `<h2/>
                                                <hr/>`);
                    $('#modal-previewConfirmPolicy').modal('show');


                    $('#modal-previewConfirmPolicy .btn-okConfirm').off('click').on('click', function (e) {
                        $('#modal-previewConfirmPolicy').modal('hide');

                        if (new Date($.now()) > endDate) {
                            showAlertModal('Your exam not start<br/> Schedule: ' + res.testScheduleStart + `to ` + res.testScheduleEnd, "schedule-ended");

                            $('#modal-previewAlert-schedule-ended').off('click').on('click', function (e) {
                                window.location = '/logout';
                            });
                        } else if (new Date($.now()) < startDate) {
                            showAlertModal('Your exam not start<br/> Schedule: ' + res.testScheduleStart + `to ` + res.testScheduleEnd, "schedule-start");

                            $('#modal-previewAlert-schedule-start').off('click').on('click', function (e) {
                                window.location = '/logout';
                            });
                        }

                        else {
                            let startAlert = `
                                                <h2 class="text-2xl font-medium leading-none mt-3 mb-3">` + res.currentCategoryName + ` Examinations <h2/>
                                                <hr/>
                                                <h5 class="font-extrabold">Do you want start the Exam<h5>
                                                <hr/>`;
                            $('#modal-previewConfirm .text-modal').html(startAlert);
                            $('#modal-previewConfirm .btn-CancelModal').html(`<a href="/logout">Do it later</a`);
                            $('#modal-previewConfirm').modal('show');

                            $('#modal-previewConfirm .btn-okConfirm').off('click').on('click', function (e) {
                                ajax(`{ historyTestId: ` + res.historyTestId + `}`, startQuestQuestUrl, function (resStart) {
                                    if (resStart.Status == "Success") {
                                        createQuest(CandidateId);
                                    }
                                });

                                $('#modal-previewConfirm').modal('hide');
                            });

                            $('#modal-previewConfirm').click(function () {
                                window.location = '/logout';
                            });

                            $('#modal-previewConfirm .modal__content').on('click', function (e) {
                                e.stopPropagation();
                            });
                        }
                    });
                    $('#modal-previewConfirmPolicy').click(function () {
                        window.location = '/logout';
                    });
                    $('#modal-previewConfirmPolicy .modal__content').on('click', function (e) {
                        e.stopPropagation();
                    });
                    isContinute = true;
                    break;
                case "NotStart":
                    if (new Date($.now()) > endDate) {
                        showAlertModal('Your exam expiration<br/> Call the examiner', "schedule-ended");

                        $('#modal-previewAlert-schedule-ended').off('click').on('click', function (e) {
                            window.location = '/logout';
                        });
                    } else {
                        let startAlert = `
                                                <h2 class="text-2xl font-medium leading-none mt-3 mb-3">` + res.currentCategoryName + ` Examinations <h2/>
                                                <hr/>
                                                <h5 class="font-extrabold">Do you want start the Exam<h5>
                                                <hr/>`;
                        $('#modal-previewConfirm .text-modal').html(startAlert);
                        $('#modal-previewConfirm .btn-CancelModal').html(`<a href="/logout">Do it later</a`);
                        $('#modal-previewConfirm').modal('show');

                        $('#modal-previewConfirm .btn-okConfirm').off('click').on('click', function (e) {
                            ajax(`{ historyTestId: ` + res.historyTestId + `}`, startQuestQuestUrl, function (resStart) {
                                if (resStart.Status == "Success") {
                                    createQuest(CandidateId);
                                }
                            });

                            $('#modal-previewConfirm').modal('hide');
                        });

                        $('#modal-previewConfirm').click(function () {
                            window.location = '/logout';
                        });

                        $('#modal-previewConfirm .modal__content').on('click', function (e) {
                            e.stopPropagation();
                        });
                    }
                    isContinute = true;
                    break;
                case "Continue":

                    if (new Date($.now()) > endDate) {
                        showAlertModal('Your exam expiration<br/> Call the examiner', "schedule-ended");

                        $('#modal-previewAlert-schedule-ended').off('click').on('click', function (e) {
                            window.location = '/logout';
                        });
                    } else {

                        if (isContinute == false) {
                            var continuteAlert = `
                                     <div class="side-nav__devider my-6"></div>
                                     <p class="text-2xl text-theme-1 font-medium leading-none">Current part: `+ res.currentCategoryName + ` </p>
                                      <div class="side-nav__devider my-6"></div>
                                      <div class="time-remaining-content"></div>
                                      <div class="side-nav__devider my-6 "></div>
                                       <p class="text-lg text-theme-1 font-medium leading-none">Schedule: `+ res.testScheduleStart + `<br/>To ` + res.testScheduleEnd + `</p>
                                    `;
                            showAlertModal(continuteAlert, "continue");
                        }

                        ajax(`{ historyTestId: ` + res.historyTestId + `}`, createQuestQuestUrl, function (resQuestContent) {
                            let content = ``;
                            let menu = ``;
                            let menumobile = ``;

                            $.each(resQuestContent.data, function (i, q) {
                                //add content
                                content += `
                                            <div class="box question mt-5" id="q_`+ q.Id + `">
                                                <div class="pb-3 pt-3 pr-2 pl-2">
                                                    <div class="flex items-center">`+ (i + 1) + `.&nbsp;&nbsp;` + q.Name + `&nbsp;&nbsp; [Choose ` + q.countCorect + ` Answer(s)]&nbsp;&nbsp;[` + q.Mark + ` Mark(s)] </div>
                                                </div>
                                              <input value="`+ q.Id + `" hidden class="questId">`;


                                $.each(q.Answers, function (j, a) {
                                    switch (q.countCorect) {
                                        case "1":
                                            content +=
                                                `<div class="awnsers">
                                                <div class="border-b border-t text-gray-600 border-gray-200 flex items-center">

                                                    <div class="flex items-center justify-center p-3">
                                                        <input  name="q_`+ q.Id + `"  value="` + a.AnswerInQuestion + `" type="radio" class="corect input border border-gray-500">
                                                    </div>

                                                    <div class="p-2"> `+ a.Name + ` </div>
                                                </div>
                                            </div>`;
                                            break;
                                        default:
                                            content +=
                                                `<div class="awnsers">
                                                <div class="border-b border-t text-gray-600 border-gray-200 flex items-center">

                                                    <div class="flex items-center justify-center p-3">
                                                        <input name="q_`+ q.id + `" value="` + a.AnswerInQuestion + `" type="checkbox" class="corect input border border-gray-500">
                                                    </div>

                                                    <div class="p-2"> `+ a.Name + ` </div>
                                                </div>
                                            </div>`;
                                            break;
                                    }
                                });
                                content += `</div>`;

                                // add menu
                                menu +=
                                    `<li>
                                             <a href="#q_`+ q.Id + `" class="side-menu" target="_self">
                                               <div class="side-menu__icon"> `+ (i + 1) + `. </div>
                                               <div class="side-menu__title"> Question `+ (i + 1) + ` </div>
                                             </a>
                                         </li>`;
                                menumobile += `
                                        <li>
                                            <a href="#q_`+ q.Id + `" class="menu" target="_self">
                                                <div class="menu__icon"> `+ (i + 1) + `. </div>
                                                <div class="menu__title">  Question `+ (i + 1) + `</div>
                                            </a>
                                        </li>`;

                            });
                            $("#aptitutest_form").html(content);
                            $("#sidemenu_content_" + res.currentCategoryId).html(menu);
                            $(".sidemenu_mobile_content").html("");
                            $("#sidemenu_mobile_content_" + res.currentCategoryId).html(menumobile);

                            historyTestId = res.historyTestId;

                            createMenu(res.currentCategoryId);

                            let sheduleEnd = new Date(parseInt(res.testScheduleEndValue.replace('/Date(', '')));
                            checkTime(res.timeSecond, sheduleEnd);

                        })
                    }

                    break;
                case "AllOver":
                    let htmlOver = `<h3 class="font-medium  text-2xl text-center mb-2 text-theme-6">Result</h3>  <div class="overflow-x-auto">
                             <table class="table">
                                 <thead>
                                     <tr>
                                         <th class="border-b-2 whitespace-no-wrap">Part</th>
                                         <th class="border-b-2 whitespace-no-wrap">Correct</th>
                                         <th class="border-b-2 whitespace-no-wrap">Mark</th>
                                         <th class="border-b-2 whitespace-no-wrap " colspan="2">Time Spent</th>
                                     </tr>
                                 </thead>
                                 <tbody>`;
                    let totalMarkR = 0;
                    $.each(res.data, function (i, d) {
                        var timespent = "";
                        if (d.DateEndTest == "") {
                            timespent = "expired";

                        } else {
                            timespent = Math.floor(d.TotalTime / 60) + ":" + (d.TotalTime - (Math.floor(d.TotalTime / 60) * 60)) + "s";
                        }
                        totalMarkR += d.PercentMark;

                        htmlOver += `<tr class="">
                                <td class="border-b">` + d.CategoryExam + `</td>
                                <td class="border-b">` + d.CorectMark + `/` + d.TotalMark + `</td>
                                <td class="border-b">` + d.PercentMark + `% </td>
                                <td class="border-b" >` + timespent + `</td>
                                <td class="border-b">` + d.DateStartTest + `<br/>to ` + d.DateEndTest + `</td>
                                 </tr>`;
                    });
                    let averageMarkR = (totalMarkR / 3).toFixed(2);
                    htmlOver += `<td class="border-b" colspan="2"><strong>Grade Point Average:</strong> </td>
                            <td class="border-b"><strong>` + averageMarkR + `%</strong></td>
                            <td class="border-b" colspan="2"><strong>if your GPA >= 80%, your will clear this round</strong></td>`;
                    htmlOver += `</tbody></table></div>`;

                    if (averageMarkR > 80) {
                        htmlOver += `<h3 class="font-medium  text-lg  text-center mb-2 mt-2 text-theme-6">You have cleared this round, next round would be HR Round</h3> `;
                    } else {
                        htmlOver += `<h3 class="font-medium  text-lg  text-center mb-2 mt-2 text-theme-6">Unfortunate, You have not cleared this round</h3> `;
                    }
                    showAlertModal(htmlOver, "all-over");
                    $('#modal-previewAlert-all-over').off('click').on('click', function (e) {
                        window.location = '/logout';
                    });

                    break;
            }
        });


    };

    function createMenu(id) {

        let clear = $(".memu-with-child");

        clear.removeClass("side-menu-with-child-open");
        clear.find(".side-menu").removeClass("side-menu--open side-menu--active side-menu-x");
        clear.find(".side-menu").attr("onclick", "");


        clear.find("ul").removeClass("side-menu__sub-open");
        clear.find("ul").css("display", "none");

        let element = $(".sidemenu_" + id);

        element.addClass("side-menu-with-child-open");
        element.find(".side-menu").addClass("side-menu--open side-menu--active side-menu-x");
        element.find(".side-menu").attr("onclick", "sideMenu(this)");


        element.find("ul").addClass("side-menu__sub-open");
        element.find("ul").css("display", "block");

    }


    let endtime;
    let shedulelasttime;
    function checkTime(seconndlast, shedulelast) {
        clearInterval(countdown);

        endtime = new Date($.now());
        endtime.setSeconds(endtime.getSeconds() + seconndlast);
        shedulelasttime = shedulelast;
        let sheduleTimeProsess = Math.floor((shedulelasttime - new Date($.now())) / 1000);
        countdown = setInterval(countDown, 500);

        $(".count-down-percent").css("width", "100%");

        $(".count-down-percent").css("-webkit-animation", "none");

        setTimeout(function () {
            $('.count-down-percent').css("-webkit-animation", seconndlast + "s" + " linear 0s 1 normal backwards running countdown");
        }, 10);

        if (sheduleTimeProsess < 900) {
            $(".count-down-percent").parent().css("top", "-16px");
            $(".count-down-percent-shedule").parent().css("top", "-10px");

            $(".count-down-percent-shedule").css("width", "100%");

            $(".count-down-percent-shedule").css("-webkit-animation", "none");


            setTimeout(function () {
                $('.count-down-percent-shedule').css("-webkit-animation", sheduleTimeProsess + "s" + " linear 0s 1 normal backwards running countdown");
            }, 10);
        }


    }
    function countDown() {
        let timeShow = [];
        let timeSheduleShow = [];
        let seconndCount = Math.floor((endtime - new Date($.now())) / 1000);
        let minuteShow = Math.floor(seconndCount / 60);
        let secondShow = seconndCount - minuteShow * 60;

        timeShow[1] = (secondShow.toString().length == 1) ? ("0" + secondShow.toString()) : secondShow.toString();
        timeShow[0] = (minuteShow.toString().length == 1) ? ("0" + minuteShow.toString()) : minuteShow.toString();

        if (seconndCount < 0) {
            $('.modal').modal('hide');
            submitForm($(".side-menu.side-menu--open .side-menu__title").html().toString());
        }


        let sheduleTimeCount = Math.floor((shedulelasttime - new Date($.now())) / 1000);

        let minuteSheduleShow = Math.floor(sheduleTimeCount / 60);
        let secondSheduleShow = sheduleTimeCount - minuteSheduleShow * 60;

        timeSheduleShow[1] = (secondSheduleShow.toString().length == 1) ? ("0" + secondSheduleShow.toString()) : secondSheduleShow.toString();

        timeSheduleShow[0] = (minuteSheduleShow.toString().length == 1) ? ("0" + minuteSheduleShow.toString()) : minuteSheduleShow.toString();

        if (sheduleTimeCount < 0) {
            submitForm($(".side-menu.side-menu--open .side-menu__title").html().toString());
        }
        $(".count-down").html(timeShow.join(":"));
        $("#modal-previewAlert-continue .time-remaining").html(timeShow.join(":"));

        if (sheduleTimeCount < 900) {
            $(".top-bar-time").append(`
                        <div class="align-middle bg-yellow-600 rounded relative w-full h-full">
                            <div class="w-full count-down-percent-shedule h-full bg-theme-6 rounded flex justify-center align-middle "></div>
                            <p class="rounded count-down-shedule h-auto text-lg text-white absolute">`+ "Shedule End " + timeSheduleShow.join(":") + `</p>
                        </div>`);
        }

    };


    function ajax(data, url, callback) {

        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (res) {
                try {
                    res.data = $.parseJSON(res.data)
                } catch (e) {

                }

                callback(res);
            }
        });
    }

    $('.button-logout').on("click", function () {

        $('#modal-previewConfirm-logout').modal('show');

        $('#modal-previewConfirm-logout').click(function () {
            $('#modal-previewConfirm-logout').modal('hide');
        });

    });

});

function showAlertModal(text, idname) {
    //$('#modal-previewAlert .text-modal').text(text);

    let modal = $('#modal-previewAlert').clone();
    $("#modal-previewAlert-" + idname).remove();
    modal.attr("id", "modal-previewAlert-" + idname);
    modal.find('.text-modal').html(text);
    if (idname == "all-over" || idname == "end-schedule") {
        modal.find(".modal__content").addClass("modal__content--xl");
        modal.find("svg").remove();
        modal.find(".p-5.text-center").addClass("score-modal");
        modal.find(".p-5.text-center").removeClass("text-center");
    }
    if (idname == "continue") {
        modal.find("svg").remove();

        modal.find(".modal__content").find(".time-remaining-content").prepend("<div class='text-lg text-theme-6 font-medium leading-none '><span >Time remaining: </span><span class='time-remaining'></span><div>");
        modal.find(".modal__content").find("button").text("Continue");

    }


    modal.modal('show');
}

function sideMenu(element) {

    if ($(element).parent().find('ul').length) {
        if ($(element).parent().find('ul').first().is(':visible')) {
            $(element).find('.side-menu__sub-icon').removeClass('transform rotate-180');
            $(element).parent().removeClass('side-menu-with-child-open');

            $(element).removeClass('side-menu--open');

            $(element).parent().find('ul').first().slideUp({
                done: function done() {
                    $(element).removeClass('side-menu__sub-open');
                }
            });
        } else {
            $(element).find('.side-menu__sub-icon').addClass('transform rotate-180');
            $(element).parent().addClass('side-menu-with-child-open');
            $(element).addClass('side-menu--open');
            $(element).parent().find('ul').first().slideDown({
                done: function done() {
                    $(element).addClass('side-menu__sub-open');
                }
            });
        }
    }
}