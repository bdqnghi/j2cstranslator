// 
// J2CsMapping : runtime library for J2CsTranslator
// 
// Copyright (c) 2008-2010 Alexandre FAU.
// All rights reserved. This program and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html
// Contributors:
//   Alexandre FAU (IBM)
//

using System;
using System.Reflection;
using System.Collections;
//using System.Runtime.Remoting;
using System.IO;
using System.Security.Policy;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;
using System.Runtime.Remoting;

namespace ILOG.J2CsMapping.Reflect 
{
	/// <summary>
	/// AssemblyScanner
	/// </summary>
	public class AssemblyScanner : IDisposable 
	{
		private bool		_disposed;				
		private AppDomain	_domain;
		private Caller      _caller;
		private static string USER_ASSEMBLIES_PROPERTY = "ilog.userAssemblies";

		public  static AssemblyScanner Self;			
		private static ArrayList candidatesAssemblies = new ArrayList();		
		private static string AssemblyFilter = "*.dll";

		/// <summary>
		/// Initialize engine search path
		/// </summary>
		public static void Initialize() 
		{				
			// BR2004.19
			string basedir = AppDomain.CurrentDomain.BaseDirectory;		
			string engineAssemblyLocation  = basedir;			
			//
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			ArrayList loadedAssembliesNames = new ArrayList();			
			foreach(Assembly ass in assemblies) 
			{																		
				loadedAssembliesNames.Add(ass.GetName());									
			}
			// engineAssemblyLocation can be "http://" ....
			// afau: BRN-2420 / BR2005.40 
			if (engineAssemblyLocation != null && engineAssemblyLocation.StartsWith("http")) {	
				string sAttr = ConfigurationSettings.AppSettings.Get(USER_ASSEMBLIES_PROPERTY);
				if (sAttr != null) {
					foreach(string s in sAttr.Split(';')) {
						if (s != null) {
							candidatesAssemblies.Add(basedir + s);
						}
					}
				}								
			} else {
				string assemblySearchDirectory = new FileInfo(engineAssemblyLocation).DirectoryName;		
				ScanSearchPath(assemblySearchDirectory, loadedAssembliesNames);		
			}				
		}
	
		/// <summary>
		/// Scan the engine search path to found candidates assemblies.
		/// </summary>
		static void ScanSearchPath(string directory, ArrayList loadedAssembliesNames) 
		{				
			FileInfo[] files = new DirectoryInfo(directory).GetFiles(AssemblyFilter);
			ArrayList lass = new ArrayList();
			foreach(FileInfo fi in files)  
			{
				Uri candidate = new Uri(fi.FullName);							
				bool alreadyLoaded = false;
				// Trace.WriteLine("Candidate = " + fi.FullName + " " + candidate);
				foreach(AssemblyName aname in loadedAssembliesNames) 
				{
					// 17/11/04 : BR2004.19 by shillion
					if (aname.CodeBase != "") 
					{
						if (new Uri(aname.CodeBase).AbsoluteUri.ToLower() == 
							candidate.AbsoluteUri.ToLower()) 
						{							
							alreadyLoaded = true;
						}
					}
				}
				if (!alreadyLoaded) 
				{					
					candidatesAssemblies.Add(candidate.AbsoluteUri);
				}				
			}
			DirectoryInfo[] directories = new DirectoryInfo(directory).GetDirectories();
			foreach(DirectoryInfo di in directories)  
			{				
				ScanSearchPath(di.FullName, loadedAssembliesNames);
			}		
		}

		public static void DisposeScanner() 
		{
			Self.Dispose();
			Self = null;
		}

		//
		//
		//

		#region CONSTRUCTORS

		public AssemblyScanner() {							
				CreateAppDomain();
				_caller = (Caller) _domain.CreateInstanceAndUnwrap(
						typeof(Caller).Assembly.FullName, typeof(Caller).FullName);								
		}

		~AssemblyScanner() 
		{
			Dispose(false);
		}
		#endregion

		#region DISPOSE

		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) 
		{	
			if (!_disposed) 
			{
				if (disposing) 
				{
					if (_domain != null) 
					{
						AppDomain.Unload(_domain);
					}
				}
				_disposed = true;				
			}
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Type Resolve(string name) 
		{				
			foreach (string candidate in candidatesAssemblies) 
			{
				try 
				{															
					AssemblyName assname = Scan(candidate, name);
					if (assname != null) 
					{
						Assembly res = AppDomain.CurrentDomain.Load(assname);
						candidatesAssemblies.Remove(candidate);
						return res.GetType(name);						
					} 						
				} 
				catch (Exception) 
				{
				}					
			}										
			return null;
		}		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="assemblyCodeBase"></param>
		/// <param name="typename"></param>
		/// <returns></returns>
		protected AssemblyName Scan(string assemblyCodeBase, string typename) 
		{
			try 
			{
				_caller.AssemblyCodeBase = assemblyCodeBase;			
				return _caller.Scan(typename);	
			} 
			catch(RemotingException e) 
			{
				// TODO : Understand why we can have a remoting exception here !
				//#if UNIT_TESTS
				throw new Exception("Remotingerror while trying to load Assembly file " + 
					assemblyCodeBase + " for type " + typename + " : " + e);
				//#endif
				//return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected void CreateAppDomain() 
		{							
			// afau: BR2005.XXX 
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; //uri.AbsolutePath;
			string name = /*codeBase +*/ "#Loader";
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationName = name;
			setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			//setup.PrivateBinPath  = baseDirectory;	

			// Create a new application domain.
            // BRN-2823	
			_domain = AppDomain.CreateDomain(name, null, setup, new PermissionSet(PermissionState.Unrestricted));			
		}

		//
		// Internal Class
		//
		
		#region CALLER

		/// <summary>
		/// 
		/// </summary>
		public class Caller : MarshalByRefObject 
		{
			public string		_assemblyCodeBase;
			private Assembly	_assembly;

			/// <summary>
			/// Intiializes a new <c>Caller</c>.
			/// </summary>
			public Caller() 
			{				
			}

			/// <summary>
			/// Sets the path to the assembly that is used as the argument to the callback method.
			/// </summary>
			public string AssemblyCodeBase 
			{
				set 
				{ 
					_assemblyCodeBase	= value; 
					_assembly			= null;
				}
			}

			public AssemblyName Scan(string typename) 
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;
				Assembly assembly = GetAssembly();
				AssemblyName result = (assembly != null) ? Scan(assembly, typename) : null;
				//Console.WriteLine("scaning to found " + typename + " in " + _assemblyCodeBase + " = " + result);
				return result;			
			}

			private AssemblyName Scan(Assembly ass, string typename) 
			{			
				if (ass.GetType(typename) != null) 
				{
					return ass.GetName();
				} 
				else
					return null;
			}

			private Assembly GetAssembly() 
			{
				if (_assembly == null) 
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					try 
					{ 
						// BUG: Can't read path with "%20" inside !
						_assembly = Assembly.LoadFrom(_assemblyCodeBase.Replace("%20"," "));						
					} 
					catch(Exception) 
					{
						//throw e;
					}
				}
				return _assembly;
			}			

			#endregion
		}
	}
}

