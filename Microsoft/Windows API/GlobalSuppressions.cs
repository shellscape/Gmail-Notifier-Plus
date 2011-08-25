// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 


#region CA1709 Identifers should use proper casing - "API" acronym chosen for clarity
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "API")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPI.ApplicationServices", MessageId = "API")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPI.Net", MessageId = "API")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPI.Shell.PropertySystem", MessageId = "API")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPI.Dialogs", MessageId = "API")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPI.Internal", MessageId = "API")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "namespace", Target = "Microsoft.WindowsAPI.Shell.PropertySystem", MessageId = "API")]
#endregion


#region CA1709 - Identifiers should use proper casing - "v" for "Version" chosen for clarity
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv6Internet", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv6NoTraffic", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv4Subnet", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv4NoTraffic", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv4Internet", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv4LocalNetwork", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv6Subnet", MessageId = "Pv")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Scope = "member", Target = "Microsoft.WindowsAPI.Net.ConnectivityStates.#IPv6LocalNetwork", MessageId = "Pv")]
#endregion

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "Microsoft.WindowsAPI.ApplicationServices.PowerPersonalityGuids.#All")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#.cctor()")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "Microsoft.WindowsAPI.Resources.LocalizedMessages.resources", MessageId = "comctl")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", Scope = "resource", Target = "Microsoft.WindowsAPI.Resources.LocalizedMessages.resources", MessageId = "Wh")]

#region CA1811 - Potentially uncalled code
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Shell.PropertySystem.PropVariantNativeMethods.#InitPropVariantFromPropVariantVectorElem(Microsoft.WindowsAPI.Internal.PropVariant,System.UInt32,Microsoft.WindowsAPI.Internal.PropVariant)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.DialogsDefaults.#get_Caption()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.DialogsDefaults.#get_Content()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreErrorHelper.#HResultFromWin32(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreErrorHelper.#Failed(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods+Size.#get_Height()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods+Size.#set_Height(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods+Size.#set_Width(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods+Size.#get_Width()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.DialogsDefaults.#get_MainInstruction()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialogSettings.#get_InvokeHelp()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialog.#get_SelectedRadioButtonId()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.TaskDialogNativeMethods+IconUnion.#get_MainIcon()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,Microsoft.WindowsAPI.Internal.WindowMessage,System.IntPtr,System.IntPtr)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,System.UInt32,System.IntPtr,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,System.UInt32,System.Int32,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#GetLoWord(System.Int64)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#GetHiWord(System.Int64,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#PostMessage(System.IntPtr,Microsoft.WindowsAPI.Internal.WindowMessage,System.IntPtr,System.IntPtr)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreNativeMethods.#SendMessage(System.IntPtr,System.UInt32,System.Int32&,System.Text.StringBuilder)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.CoreErrorHelper.#Matches(System.Int32,System.Int32)")]
#endregion


[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1014:MarkAssembliesWithClsCompliant", Justification = "There are places where unsigned values are used, which is considered not Cls compliant.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.WindowsAPI.Shell.PropertySystem", Justification = "Uses nested classes to organize the namespce, there is a workitem to resolve this.")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Scope = "type", Target = "Microsoft.WindowsAPI.Dialogs.TaskDialogResult", Justification = "Does not represent flags.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#Value", Justification = "Uses a switch statement.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Justification = "These return snapshots of a state, which are likely to change between calls.", Target = "Microsoft.WindowsAPI.ApplicationServices.PowerManager.#GetCurrentBatteryState()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Justification = "These return snapshots of a state, which are likely to change between calls.", Target = "Microsoft.WindowsAPI.Net.NetworkListManager.#GetNetworkConnections()")]

#region CA2122, CA2123 - LinkDemand related
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#.ctor(System.Single[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#.ctor(System.Decimal[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.ApplicationServices.ApplicationRestartRecoveryManager.#RegisterForApplicationRecovery(Microsoft.WindowsAPI.ApplicationServices.RecoverySettings)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialog.#AllocateAndMarshalButtons(Microsoft.WindowsAPI.Dialogs.TaskDialogNativeMethods+TaskDialogButton[])")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialog.#Dispose(System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialog.#FreeOldString(Microsoft.WindowsAPI.Dialogs.TaskDialogNativeMethods+TaskDialogElements)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialog.#MakeNewString(System.String,Microsoft.WindowsAPI.Dialogs.TaskDialogNativeMethods+TaskDialogElements)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialog.#NativeShow()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.NativeTaskDialogSettings.#.ctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#GetBlobData()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#.ctor(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.PropVariant.#Value")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.SafeWindowHandle.#ReleaseHandle()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.ApplicationServices.Power.#GetSystemBatteryState()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.ApplicationServices.Power.#GetSystemPowerCapabilities()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.ZeroInvalidHandle.#.ctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.SafeIconHandle.#ReleaseHandle()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.SafeRegionHandle.#ReleaseHandle()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.SafeWindowHandle.#ReleaseHandle()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase", Scope = "member", Target = "Microsoft.WindowsAPI.Internal.ZeroInvalidHandle.#IsInvalid")]
#endregion

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Scope = "member", Target = "Microsoft.WindowsAPI.Dialogs.TaskDialogNativeMethods.#TaskDialogIndirect(Microsoft.WindowsAPI.Dialogs.TaskDialogNativeMethods+TaskDialogConfiguration,System.Int32&,System.Int32&,System.Boolean&)", Justification="This does exist, I believe it is caused by FxCop using the wrong version of the DLL.")]
