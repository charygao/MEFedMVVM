using System;

namespace MEFedMVVM.ViewModelLocator
{
    /// <summary>
    /// Attribute to be used on assemblies that you want to ignore at design time
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class IgnoreAtDesignTimeAttribute : Attribute
    { }
}