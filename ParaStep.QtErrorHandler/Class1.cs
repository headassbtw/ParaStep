using System;
using System.IO;
using ParaStep.PRK;
using ParaStep.QtErrorHandler.QML_Types;
using Qml.Net;
using Qml.Net.Runtimes;

namespace ParaStep.QtErrorHandler
{
    public static class Program
    {
        public static Exception Exception;
        public static Interface QtInterface = new Interface(Path.Combine(Environment.CurrentDirectory, "res", "qt.prk"));
        public static int Fatal(Exception exception)
        {
            
            Exception = exception;
            RuntimeManager.DiscoverOrDownloadSuitableQtRuntime();
            QQuickStyle.SetStyle("Material");
            using (var app = new QGuiApplication(new string[0]))
            {
                
                using (var engine = new QQmlApplicationEngine())
                {
                    Qml.Net.Qml.RegisterType<NetExceptionModel>("net", 1, 1);
                    Qml.Net.Qml.RegisterType<NetExitHandler>("net", 1, 1);
                    string f = QtInterface.ExtractTmpFile("Test.qml");
                    Console.WriteLine($"Loading QML file {f}");
                    engine.Load(f);
                    return app.Exec();
                }
            }
        }

        internal static QGuiApplication UserCrashApp;
        internal static QQmlApplicationEngine UserCrashEngine;
        public static int UserScript(Exception exception)
        {
            Exception = exception;
            RuntimeManager.DiscoverOrDownloadSuitableQtRuntime();
            QQuickStyle.SetStyle("Material");
            UserCrashApp = new QGuiApplication(new string[0]);
            UserCrashEngine = new QQmlApplicationEngine();
            
            
            Qml.Net.Qml.RegisterType<NetExceptionModel>("net", 1, 1);
            Qml.Net.Qml.RegisterType<NetExitHandler_User>("net", 1, 1);
            string f = QtInterface.ExtractTmpFile("Test_nonfatal.qml");
            UserCrashEngine.Load(f);
            return UserCrashApp.Exec();
        }
    }
}