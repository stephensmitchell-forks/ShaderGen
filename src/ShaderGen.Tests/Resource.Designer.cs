﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShaderGen.Tests {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ShaderGen.Tests.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to parameters.$$NAME$$.
        /// </summary>
        internal static string SBSArgs {
            get {
                return ResourceManager.GetString("SBSArgs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to case $$CASE$$:
        ///                    parameters.$$OUTPUT$$ = $$METHOD$$($$ARGS$$);
        ///                    break;.
        /// </summary>
        internal static string SBSCase {
            get {
                return ResourceManager.GetString("SBSCase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to public class ComputeShader
        ///    {
        ///        /// &lt;summary&gt;
        ///        /// The number of methods.
        ///        /// &lt;/summary&gt;
        ///        public const uint Methods = .
        /// </summary>
        internal static string SBSP1 {
            get {
                return ResourceManager.GetString("SBSP1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;
        ///
        ///        [ResourceSet(0)] public RWStructuredBuffer&lt;ComputeShaderParameters&gt; InOutParameters;
        ///
        ///        [ComputeShader(1, 1, 1)]
        ///        public void CS()
        ///        {
        ///            DoCS(DispatchThreadID);
        ///        }
        ///
        ///        /*
        ///         * TODO Issue #67 - WORKAROUND until DispatchThreadID is removed and the parameter style implemented
        ///         */
        ///        public void DoCS(UInt3 dispatchThreadID)
        ///        {
        ///            int index = (int)dispatchThreadID.X;
        ///
        ///            // ReSharper disable once Redu [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SBSP2 {
            get {
                return ResourceManager.GetString("SBSP2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to             }
        ///
        ///            InOutParameters[index] = parameters;
        ///        }
        ///    }
        ///
        ///    [StructLayout(LayoutKind.Sequential)]
        ///    public unsafe struct ComputeShaderParameters
        ///    {.
        /// </summary>
        internal static string SBSP3 {
            get {
                return ResourceManager.GetString("SBSP3", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to }.
        /// </summary>
        internal static string SBSP4 {
            get {
                return ResourceManager.GetString("SBSP4", resourceCulture);
            }
        }
    }
}
