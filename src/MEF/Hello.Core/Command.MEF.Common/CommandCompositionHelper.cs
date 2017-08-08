using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Command.MEF.Contracts;

namespace Command.MEF.Common
{
    public class CommandCompositionHelper
    {
        public CommandCompositionHelper(string componentFolder)
        {
            ComponentFolder = componentFolder;
        }

        public string ComponentFolder { get; set; }

        [ImportMany]
        public System.Lazy<ICommandAssembly, IDictionary<string, object>>[] CommandAssemblyPlugins { get; set; }

        /// <summary>
        /// Assembles the Command components
        /// </summary>
        public void AssembleCommandComponents()
        {
            #region First Example
            /**
                try
                {
                    //Step 1: Initializes a new instance of the 
                    //        System.ComponentModel.Composition.Hosting.AssemblyCatalog  
                    //        class with the current executing assembly.
                    var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

                    //Step 2: The assemblies obtained in step 1 are added to the 
                    //CompositionContainer
                    var container = new CompositionContainer(catalog);

                    //Step 3: Composable parts are created here i.e. 
                    //the Import and Export components 
                    //        assemble here
                    container.ComposeParts(this);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                /**/
            #endregion

            try
            {
                //Creating an instance of aggregate catalog. It aggregates other catalogs
                var aggregateCatalog = new AggregateCatalog();

                //Build the directory path where the parts will be available
                string directoryPath = ComponentFolder;

                //Load parts from the available DLLs in the specified path 
                //using the directory catalog
                var directoryCatalog = new DirectoryCatalog(directoryPath, "*.dll");

                //Load parts from the current assembly if available
                var asmCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

                //Add to the aggregate catalog
                aggregateCatalog.Catalogs.Add(directoryCatalog);
                aggregateCatalog.Catalogs.Add(asmCatalog);

                //Crete the composition container
                var container = new CompositionContainer(aggregateCatalog);

                // Composable parts are created here i.e. 
                // the Import and Export components assembles here
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Initialize()
        {
            foreach (var plugin in CommandAssemblyPlugins)
            {
                plugin.Value.Initialize();
            }
        }
      
    }
}