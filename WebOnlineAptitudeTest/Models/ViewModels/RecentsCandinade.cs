using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Models.ViewModels
{
    public class RecentsCandinade
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime ParticipateDate { get; set; }
        public DateTime TranferDate { get; set; }

        public double? GeneralKnowledge { get; set; }
        public double? Mathematics { get; set; }
        public double? ComputerTechnology { get; set; }
        public double AverageMark
        {
            get { return System.Math.Round((GeneralKnowledge.Value + Mathematics.Value + ComputerTechnology.Value) / 3, 2); }   // get method
            set { averageMark = value; }  // set method
        }

        private double averageMark;


    }
}