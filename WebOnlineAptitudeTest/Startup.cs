using CKSource.CKFinder.Connector.Config;
using CKSource.CKFinder.Connector.Core.Builders;
using CKSource.CKFinder.Connector.Host.Owin;
using CKSource.FileSystem.Local;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(WebOnlineAptitudeTest.Startup))]

namespace WebOnlineAptitudeTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            FileSystemFactory.RegisterFileSystem<LocalStorage>();
            app.Map("/ckfinder/connector", SetupConnector);
        }
        private static void SetupConnector(IAppBuilder app)
        {
            /*
             * Create a connector instance using ConnectorBuilder. The call to the LoadConfig() method
             * will configure the connector using CKFinder configuration options defined in Web.config.
             */
            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();

            /*
             * Create an instance of authenticator implemented in the previous step.
             */
            var customAuthenticator = new CustomCKFinderAuthenticator();


            connectorBuilder
                /*
                 * Provide the global configuration.
                 *
                 * If you installed CKSource.CKFinder.Connector.Config, you should load the static configuration
                 * from XML:
                 * connectorBuilder.LoadConfig();
                 */
                .LoadConfig()
                .SetAuthenticator(customAuthenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        /*
                         * If you installed CKSource.CKFinder.Connector.Config, you might want to load the static
                         * configuration from XML as a base configuration to modify:
                         */
                        config.LoadConfig();

                    }
                );

            /*
             * Build the connector middleware.
             */
            var connector = connectorBuilder
                .Build(connectorFactory);

            /*
             * Add the CKFinder connector middleware to the web application pipeline.
             */
            app.UseConnector(connector);
        }
    }
}
