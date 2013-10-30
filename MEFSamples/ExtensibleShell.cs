using SamplesAPI;
using Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace MEFSamples
{
	// Builtin command
	[Export(typeof(ICommand))]
	internal class Help : ICommand
	{
		public string Name
		{
			get
			{
				return "help";
			}
		}
		
		public string Documentation
		{
			get
			{
				return "Print some information about a command.";
			}
		}
		
		public void Execute(IShell shell, string[] parameters)
		{
			if (parameters.Count() == 0)
			{
				Console.WriteLine("The availables commands are :");
		
				shell.Commands.ToList().ForEach(command => Console.WriteLine("- {0}", command.Name));
			}
			else
			{
				Console.WriteLine(shell.Commands.Where(command => command.Name == parameters[0]).Single().Documentation);
			}
		}
	}
	
	// Builtin command
	[Export(typeof(ICommand))]
	internal class Exit : ICommand
	{
		public string Name
		{
			get
			{
				return "exit";
			}
		}
		
		public string Documentation
		{
			get
			{
				return "Exit the current shell.";
			}
		}
		
		public void Execute(IShell shell, string[] parameters)
		{
			shell.Stop();
		}
	}
	
	// Builtin command
	[Export(typeof(ICommand))]
	internal class Addins : ICommand
	{
		public string Name
		{
			get
			{
				return "addins";
			}
		}
		
		public string Documentation
		{
			get
			{
				return "Load some additional commands.";
			}
		}
		
		public void Execute(IShell shell, string[] parameters)
		{
			// Import the addin commands.
            (shell as ExtensibleShell).CommandsCatalog.Catalogs.Add(new DirectoryCatalog(parameters.Length == 1 ? parameters[0] : "./addins"));
		}
	}

	public class ExtensibleShell : IShell
	{
		[ImportMany(AllowRecomposition = true)]
		public IEnumerable<ICommand> Commands
		{
			get;
			private set;
		}
		
		public bool IsRunning
		{
			get;
			private set;
		}
	
		internal AggregateCatalog CommandsCatalog
		{
			get;
			private set;
		}
	
		public void Run()
		{
			this.IsRunning = true;
		
			this.CommandsCatalog = new AggregateCatalog();
			// Only reference the builtin commands, addin commands will be loaded on demand.
			this.CommandsCatalog.Catalogs.Add(new TypeCatalog(typeof(Help), typeof(Addins), typeof(Exit)));
		
			CompositionContainer commandsContainer = new CompositionContainer(this.CommandsCatalog);
			commandsContainer.ComposeParts(this);
		
			do
			{
				Console.Write("> ");
			
				string input = Console.ReadLine();
			
				if (!String.IsNullOrEmpty(input))
				{
					string[] tokens = input.Split(' ');
				
					string commandName = tokens[0];
					string[] commandParameters = tokens.Skip(1).ToArray();
					
					IEnumerable<ICommand> matchingCommands = this.Commands.Where(command => command.Name == commandName);
					
					if (matchingCommands.Count() != 0)
					{
                        try
                        {
                            matchingCommands.Single().Execute(this, commandParameters);
                        }
                        catch (Exception e)
                        {
                            using (Color.Red)
                            {
                                Console.WriteLine("Error while executing command '{0}'!", input);
                                Console.WriteLine(e);
                            }
                        }
					}
					else
					{
                        using (Color.Yellow) Console.WriteLine("Command not found.");
					}
				}
			}
			while(this.IsRunning);
		}
		
		public void Stop()
		{
			Console.WriteLine("Bye!");
		
			this.IsRunning = false;
		}
	}
}
