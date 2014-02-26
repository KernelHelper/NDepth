;--------------------------------
;Startup parameters

!ifndef BUILD
  !define BUILD "1.0.0.0"
!endif

!ifndef CONFIGURATION
  !define CONFIGURATION "Debug"
!endif

!ifndef PLATFORM
  !define PLATFORM "x86"
!endif

!ifndef SOURCES_DIR
  !define SOURCES_DIR "..\"
!endif

!ifndef SETUP_FILENAME
  !define SETUP_FILENAME "NDepth.Setup.exe"
!endif

;--------------------------------
;Includes

  !addplugindir "Plugins"

  !include "x64.nsh"
  !include "FileFunc.nsh"
  !include "InstallOptions.nsh"
  !include "LogicLib.nsh"
  !include "MUI2.nsh"
  !include "Plugins\NSISpcre.nsh"
  !include "Resources\Resources.en.nsi"

;--------------------------------
;Basic definitions

  !define PRODUCT_NAME "NDepth"
  !define PRODUCT_BUILD "${BUILD}"
  !define PRODUCT_CONFIGURATION "${CONFIGURATION}"
  !define PRODUCT_PLATFORM "${PLATFORM}"
  !define PRODUCT_PUBLISHER "The NULL workgroup"
  !define PRODUCT_WEB_SITE "code.google.com/p/ndepth/"
  !define PRODUCT_WEB_SITE_URL "http://${PRODUCT_WEB_SITE}"
  !define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\${PRODUCT_NAME}"
  !define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  !define PRODUCT_UNINST_ROOT_KEY "HKLM"
  
;--------------------------------
; Global variables

var INSTALL_DOTNET

;--------------------------------
;General

  ;Setup compressor
  SetCompressor LZMA

  ;Name and file
  Name "${PRODUCT_NAME} build ${PRODUCT_BUILD} (${PRODUCT_PLATFORM} ${PRODUCT_CONFIGURATION})"
  OutFile "${SETUP_FILENAME}"

  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Installation types
  InstType "Update"
  InstType "Full"
  InstType "Examples"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  Page custom ShowMachineConfigPage LeaveMachineConfigPage
  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Plugins

  ;!insertmacro REQuoteMeta
  ;!insertmacro RECheckPattern
  ;!insertmacro REClearAllOptions
  ;!insertmacro REClearOption
  ;!insertmacro REGetOption
  ;!insertmacro RESetOption
  !insertmacro RECaptureMatches
  !insertmacro REMatches
  ;!insertmacro REReplace
  ;!insertmacro REFind
  ;!insertmacro REFindNext
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Global macro

!macro InstallMachineConfig

  SetOutPath "$INSTDIR"

  !insertmacro INSTALLOPTIONS_READ $0 "NDepth.Setup.Machine.ini" "Field 2" "State"
  !insertmacro INSTALLOPTIONS_READ $1 "NDepth.Setup.Machine.ini" "Field 4" "State"
  !insertmacro INSTALLOPTIONS_READ $2 "NDepth.Setup.Machine.ini" "Field 6" "State"
  !insertmacro INSTALLOPTIONS_READ $3 "NDepth.Setup.Machine.ini" "Field 8" "State"
  !insertmacro INSTALLOPTIONS_READ $4 "NDepth.Setup.Machine.ini" "Field 10" "State"
  !insertmacro INSTALLOPTIONS_READ $5 "NDepth.Setup.Machine.ini" "Field 11" "State"
  !insertmacro INSTALLOPTIONS_READ $6 "NDepth.Setup.Machine.ini" "Field 12" "State"
  !insertmacro INSTALLOPTIONS_READ $7 "NDepth.Setup.Machine.ini" "Field 13" "State"

  var /GLOBAL ConnectionString
  StrCpy $ConnectionString "$3"
  var /GLOBAL ConnectionType
  ${If} $4 != "0"
    StrCpy $ConnectionType ""
  ${ElseIf} $5 != "0"
    StrCpy $ConnectionType "Sqlite"
  ${ElseIf} $6 != "0"
    StrCpy $ConnectionType "SqlServer"
  ${ElseIf} $7 != "0"
    StrCpy $ConnectionType "SqlServerCe"
  ${Else}
    StrCpy $ConnectionType ""
  ${EndIf}

  ClearErrors
  FileOpen $R0 $INSTDIR\Machine.xml w
  FileWrite $R0 `<?xml version="1.0" encoding="utf-8"?>$\r$\n`
  FileWrite $R0 `<Machine>$\r$\n`
  FileWrite $R0 `  <Address>$0</Address>$\r$\n`
  FileWrite $R0 `  <Name>$1</Name>$\r$\n`
  FileWrite $R0 `  <Description>$2</Description>$\r$\n`
  FileWrite $R0 `  <ConnectionString>$ConnectionString</ConnectionString>$\r$\n`
  FileWrite $R0 `  <ConnectionType>$ConnectionType</ConnectionType>$\r$\n`
  FileWrite $R0 `  <Logging>$\r$\n`
  FileWrite $R0 `    <LoggingType>Log4Net</LoggingType>$\r$\n`
  FileWrite $R0 `    <LoggingConfig>$\r$\n`
  FileWrite $R0 `      <configType>FILE-WATCH</configType>$\r$\n`
  FileWrite $R0 `      <configFile>~/log4net.config</configFile>$\r$\n`
  FileWrite $R0 `    </LoggingConfig>$\r$\n`
  FileWrite $R0 `  </Logging>$\r$\n`
  FileWrite $R0 `</Machine>$\r$\n`
  FileClose $R0

!macroend

!macro UninstallMachineConfig

  ${If} ${FileExists} "$INSTDIR\Machine.xml"
    ${If} ${Silent}
    ${Else}
      MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON1 $(MsgUninstallMachineConfig) IDYES UninstallMachineConfigYes IDNO UninstallMachineConfigNo
      UninstallMachineConfigYes:
        Delete "$INSTDIR\Machine.xml"
      UninstallMachineConfigNo:
    ${EndIf}
  ${EndIf}

!macroend

!macro InstallUpdateDB

  SetOutPath "$INSTDIR\Database"

  File /r "${SOURCES_DIR}\Database\*.*"
  File /r "${SOURCES_DIR}\Sources\Database\Migrations\bin\${CONFIGURATION}\*.*"

  ${If} $ConnectionString != ""
  ${AndIf} $ConnectionType != ""
    ExecWait "$\"$INSTDIR\Database\MigrateUp.bat$\" $\"NDepth.Database.Migrations.dll$\" $\"$ConnectionType$\" $\"$ConnectionString$\"" $0
    ${If} $0 != 0
      Abort "$(MsgUpdateDBFail) $0"
    ${EndIf}
  ${EndIf}

!macroend

!macro UninstallUpdateDB

  Delete "$INSTDIR\Database\*.*"
  RMDir /r "$INSTDIR\Database"

!macroend

!macro InstallExamples 

  SetOutPath "$INSTDIR\Examples\Common\BlockingCollectionExample"
  File /r "${SOURCES_DIR}\Examples\Common\BlockingCollectionExample\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Common\DisposeAndFinalizeExample"
  File /r "${SOURCES_DIR}\Examples\Common\DisposeAndFinalizeExample\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Common\DisruptorExample"
  File /r "${SOURCES_DIR}\Examples\Common\DisruptorExample\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Common\HotSwapExample"
  File /r "${SOURCES_DIR}\Examples\Common\HotSwapExample\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Common\LINQToObjectsExamples"
  File /r "${SOURCES_DIR}\Examples\Common\LINQToObjectsExamples\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Common\LINQToXMLExamples"
  File /r "${SOURCES_DIR}\Examples\Common\LINQToXMLExamples\bin\${CONFIGURATION}\*.*"

  SetOutPath "$INSTDIR\Examples\Database\SQLExamples"
  File /r "${SOURCES_DIR}\Examples\Database\SQLExamples\bin\${PLATFORM}\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Database\SQLiteExample"
  File /r "${SOURCES_DIR}\Examples\Database\SQLiteExample\bin\${PLATFORM}\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Database\NHibernateSQLiteExample"
  File /r "${SOURCES_DIR}\Examples\Database\NHibernateSQLiteExample\bin\${PLATFORM}\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Database\FluentMigratorExample"
  File /r "${SOURCES_DIR}\Examples\Database\FluentMigratorExample\bin\${PLATFORM}\${CONFIGURATION}\*.*"

  SetOutPath "$INSTDIR\Examples\Logging\Simple"
  File /r "${SOURCES_DIR}\Examples\Logging\Simple\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Logging\Log4net"
  File /r "${SOURCES_DIR}\Examples\Logging\Log4net\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Logging\NLog"
  File /r "${SOURCES_DIR}\Examples\Logging\NLog\bin\${CONFIGURATION}\*.*"

  SetOutPath "$INSTDIR\Examples\Module\ModuleConsoleExample"
  File /r "${SOURCES_DIR}\Examples\Module\ModuleConsoleExample\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Module\ModuleLoggingExample"
  File /r "${SOURCES_DIR}\Examples\Module\ModuleLoggingExample\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Module\ModuleLoggingPerformance"
  File /r "${SOURCES_DIR}\Examples\Module\ModuleLoggingPerformance\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Module\ModuleMonitoringPerformance"
  File /r "${SOURCES_DIR}\Examples\Module\ModuleMonitoringPerformance\bin\${CONFIGURATION}\*.*"
  SetOutPath "$INSTDIR\Examples\Module\ModuleMonitoringExample"
  File /r "${SOURCES_DIR}\Examples\Module\ModuleMonitoringExample\bin\${CONFIGURATION}\*.*"

  SetOutPath "$INSTDIR\Examples\XML\XSLT"
  File /r "${SOURCES_DIR}\Examples\XML\XSLT\*.*"

!macroend

!macro UninstallExamples 

  RMDir /r "$INSTDIR\Examples"

!macroend

!macro InstallFinish 

  SetOutPath "$INSTDIR"

  File "${SOURCES_DIR}\Setup\NDepth.ico"

  ;Store installation folder
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\Uninstall.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\NDepth.ico"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_BUILD}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
 
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

!macroend

!macro Uninstall

  !insertmacro UninstallExamples
  !insertmacro UninstallUpdateDB
  !insertmacro UninstallMachineConfig

  Delete "$INSTDIR\NDepth.ico"
  Delete "$INSTDIR\Uninstall.exe"

  ${If} ${FileExists} "$INSTDIR\Machine.xml"
  ${Else}
    RMDir /r "$INSTDIR"
  ${EndIf}

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  
!macroend

;--------------------------------
;Installer Functions

Function CheckDotNETFramework
	
  SetOutPath "$INSTDIR"

  ; Get .NET if required
  ${If} $INSTALL_DOTNET == "1"
     
    SetDetailsView hide

    inetc::get /caption $(MsgDownloadingDotNet) /canceltext $(MsgCancelButtonText) "http://download.microsoft.com/download/b/a/4/ba4a7e71-2906-4b2d-a0e1-80cf16844f5f/dotnetfx45_full_x86_x64.exe" "$INSTDIR\dotnetfx.exe" /end
    Pop $1

    ${If} $1 != "OK"
      Delete "$INSTDIR\dotnetfx.exe"
      Abort $(MsgInstallationCanceled)
    ${EndIf}

    ExecWait "$INSTDIR\dotnetfx.exe" $2
    Delete "$INSTDIR\dotnetfx.exe"

    ${If} $2 != "0"
      Abort "$(MsgInstallationAborted) $2"
    ${EndIf}

    SetDetailsView show

  ${EndIf}

FunctionEnd

Function .onInit

  ;Default installation folder
  ${If} ${RunningX64}
    ${If} ${PLATFORM} == "x86"
      StrCpy $INSTDIR "$PROGRAMFILES32\${PRODUCT_NAME}"
    ${Else}
      StrCpy $INSTDIR "$PROGRAMFILES64\${PRODUCT_NAME}"
    ${EndIf}
  ${Else}
    StrCpy $INSTDIR "$PROGRAMFILES\${PRODUCT_NAME}"
  ${EndIf}

  ${If} ${Silent}
  ${Else}
    ; Check .NET version
    ReadRegDWORD $0 "HKLM" "Software\Microsoft\NET Framework Setup\NDP\v4\Full" "Install"
    ReadRegDWORD $1 "HKLM" "Software\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
    ${If} ${Errors}
    ${OrIf} $0 < 1
    ${OrIf} $1 < 378389
      MessageBox MB_OKCANCEL|MB_ICONINFORMATION $(MsgDotNetNotInstalled) IDOK SetNetInstallYes IDNO SetNetInstallNo
      SetNetInstallYes:
        StrCpy $INSTALL_DOTNET "1"
      SetNetInstallNo:	  
        Quit
    ${EndIf}
  ${EndIf}

  ; Check for the previous installation.
  ReadRegStr $R0 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString"
  ${If} $R0 != ""
    ; Ask for uninstall confirmation.
    StrCpy $R1 0
    ${If} ${Silent}
      StrCpy $R1 1
    ${Else}
      MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON1 $(MsgUninstallPrev) IDYES UninstallPrevYes IDNO UninstallPrevNo
      UninstallPrevYes:
        StrCpy $R1 1
      UninstallPrevNo:
    ${EndIf}
    ; Pefrom uninstallation of the previous version.
    ${If} $R1 = 1
      ClearErrors
      ${If} ${Silent}
        ExecWait '$R0 /S _?=$INSTDIR'
      ${Else}
        ExecWait '$R0 _?=$INSTDIR'
      ${EndIf}
      ${If} ${Silent}
      ${Else}
        MessageBox MB_ICONINFORMATION|MB_OK $(MsgUninstallPrevComplete)
      ${EndIf}
    ${EndIf}
  ${EndIf}

  !insertmacro INSTALLOPTIONS_EXTRACT "NDepth.Setup.Machine.ini"

FunctionEnd

Function un.onInit

FunctionEnd

Function un.onUninstSuccess

FunctionEnd

Function ShowMachineConfigPage

  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 1" "Text"  $(MsgMachineAddress)
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 2" "State" "localhost"
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 3" "Text"  $(MsgMachineName)
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 4" "State" "Local"
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 5" "Text"  $(MsgMachineDescription)
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 6" "State" "Local machine"
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 7" "Text"  $(MsgMachineConnectionString)
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 8" "State" ""
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 9" "Text"  $(MsgMachineConnectionType)
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 10" "State" "0"
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 11" "State" "0"
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 12" "State" "0"
  !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 13" "State" "0"

  ${If} ${FileExists} "$INSTDIR\Machine.xml"
    ClearErrors
    FileOpen $R0 $INSTDIR\Machine.xml r
    ${Unless} ${Errors}
      ${Do}
        FileRead $R0 $R1
        ${REMatches} $R2 "<Address>" $R1 1
        ${If} $R2 == "true"
          ${RECaptureMatches} $R3 "<Address>(.*?)</Address>" $R1 1
          ${If} $R3 == 1
            Pop $R4
            !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 2" "State" $R4
          ${EndIf}
        ${EndIf}
        ${REMatches} $R2 "<Name>" $R1 1
        ${If} $R2 == "true"
          ${RECaptureMatches} $R3 "<Name>(.*?)</Name>" $R1 1
          ${If} $R3 == 1
            Pop $R4
            !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 4" "State" $R4
          ${EndIf}
        ${EndIf}
        ${REMatches} $R2 "<Description>" $R1 1
        ${If} $R2 == "true"
          ${RECaptureMatches} $R3 "<Description>(.*?)</Description>" $R1 1
          ${If} $R3 == 1
            Pop $R4
            !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 6" "State" $R4
          ${EndIf}
        ${EndIf}
        ${REMatches} $R2 "<ConnectionString>" $R1 1
        ${If} $R2 == "true"
          ${RECaptureMatches} $R3 "<ConnectionString>(.*?)</ConnectionString>" $R1 1
          ${If} $R3 == 1
            Pop $R4
            !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 8" "State" $R4
          ${EndIf}
        ${EndIf}
        ${REMatches} $R2 "<ConnectionType>" $R1 1
        ${If} $R2 == "true"
          ${RECaptureMatches} $R3 "<ConnectionType>(.*?)</ConnectionType>" $R1 1
          ${If} $R3 == 1
            Pop $R4
            ${If} $R4 == ""
              !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 10" "State" "1"
            ${ElseIf} $R4 == "Sqlite"
              !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 11" "State" "1"
            ${ElseIf} $R4 == "SqlServer"
              !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 12" "State" "1"
            ${ElseIf} $R4 == "SqlServerCe"
              !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 13" "State" "1"
            ${Else}
              !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 10" "State" "1"
            ${EndIf}
          ${EndIf}
        ${EndIf}
        ${If} $R1 == ""
          ${ExitDo}
        ${EndIf}
      ${LoopUntil} ${Errors}
    ${EndUnless}
    FileClose $R0
  ${Else}
    !insertmacro INSTALLOPTIONS_WRITE "NDepth.Setup.Machine.ini" "Field 10" "State" "1"
  ${EndIf}

  !insertmacro MUI_HEADER_TEXT $(MsgMachinePageSection) $(MsgMachinePageDescription)

  !insertmacro INSTALLOPTIONS_DISPLAY "NDepth.Setup.Machine.ini"

FunctionEnd

Function LeaveMachineConfigPage

FunctionEnd

;--------------------------------
;Installer Sections

Section -Pre
SectionIn 1 2 3
  Call CheckDotNETFramework
  !insertmacro InstallMachineConfig
SectionEnd

Section $(MsgUpdateDBPageSection) SecUpdateDB
SectionIn 1 2 3

  !insertmacro InstallUpdateDB

SectionEnd

Section $(MsgExamplesPageSection) SecExamples
SectionIn 1 2 3

  !insertmacro InstallExamples

SectionEnd

Section -Post
SectionIn 1 2 3

  !insertmacro InstallFinish

SectionEnd

;--------------------------------
;Uninstaller Section

Section Uninstall
	!insertmacro Uninstall
SectionEnd

;--------------------------------
;Descriptions

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecUpdateDB}	$(MsgUpdateDBPageDescription)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecExamples}	$(MsgExamplesPageDescription)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END                                                  
