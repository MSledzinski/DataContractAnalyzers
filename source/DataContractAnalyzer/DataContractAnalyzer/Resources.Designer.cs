﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataContractAnalyzer {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DataContractAnalyzer.Resources", typeof(Resources).GetTypeInfo().Assembly);
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
        ///   Looks up a localized string similar to DataContract Name and class name are the same.
        /// </summary>
        internal static string DataContractAnalyzerDescription {
            get {
                return ResourceManager.GetString("DataContractAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Class name &apos;{0}&apos; is inconsistent with DataContract name parameter.
        /// </summary>
        internal static string DataContractAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("DataContractAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataContract Name and class name are the same.
        /// </summary>
        internal static string DataContractAnalyzerTitle {
            get {
                return ResourceManager.GetString("DataContractAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Change DataContract name parameter value.
        /// </summary>
        internal static string DataContractFixTitle {
            get {
                return ResourceManager.GetString("DataContractFixTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataMember attribute present in DataCotnract.
        /// </summary>
        internal static string DataMemberDescription {
            get {
                return ResourceManager.GetString("DataMemberDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add DataMember attribute.
        /// </summary>
        internal static string DataMemberFixTitle {
            get {
                return ResourceManager.GetString("DataMemberFixTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataMember attribute is missing.
        /// </summary>
        internal static string DataMemberMessageFormat {
            get {
                return ResourceManager.GetString("DataMemberMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DataMemberpresnet in DataContract.
        /// </summary>
        internal static string DataMembersAnalyzerTitle {
            get {
                return ResourceManager.GetString("DataMembersAnalyzerTitle", resourceCulture);
            }
        }
    }
}
