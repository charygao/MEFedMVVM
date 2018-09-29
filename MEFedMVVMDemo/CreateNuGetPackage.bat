xcopy "..\latest build\Release\WPF\MEFedMVVM.WPF\*.dll" "..\latest build\Release\MEFedMVVM\lib\net40\" /I /Y
xcopy "..\latest build\Release\Silverlight\MEFedMVVM.SL\MEFedMVVM.SL.dll" "..\latest build\Release\MEFedMVVM\lib\sl40\" /I /Y
xcopy "..\latest build\Release\Silverlight\MEFedMVVM.SL\System.Windows.Interactivity.dll" "..\latest build\Release\MEFedMVVM\lib\sl40\" /I /Y
xcopy "..\latest build\Release\Silverlight\MEFedMVVM.SL\Microsoft.Expression.Interactions.dll" "..\latest build\Release\MEFedMVVM\lib\sl40\" /I /Y
xcopy MEFedMVVM.nuspec "..\latest build\Release\MEFedMVVM" /Y


xcopy "..\latest build\Release\WPF\MEFedMVVM.Extensions\ValidationExtensions\*.dll" "..\latest build\Release\MEFedMVVM.ValidationExtensions\lib\net40\" /I /Y
xcopy "..\latest build\Release\Silverlight\MEFedMVVM.Extensions\ValidationExtensions\*.dll" "..\latest build\Release\MEFedMVVM.ValidationExtensions\lib\sl40\" /I /Y
xcopy MEFedMVVM.ValidationExtensions.nuspec "..\latest build\Release\MEFedMVVM.ValidationExtensions" /Y

cd "..\latest build\Release\MEFedMVVM\"
nuget pack MEFedMVVM.nuspec

cd ..\MEFedMVVM.ValidationExtensions
nuget pack MEFedMVVM.ValidationExtensions.nuspec

cd ..\..\..\MEFedMVVMDemo
