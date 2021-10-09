using System;
using ParaStep.Archive;
using ParaStep.QtErrorHandler.QML_Types;
using Qml.Net;
using Qml.Net.Runtimes;

namespace ParaStep.QtErrorHandler
{
    public static class Program
    {
        public static Exception Exception;
        public static int Main(Exception exception)
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
                    Get.File("QT/Test.qml");
                    engine.Load(Get.File("QT/Window.qml"));
                    return app.Exec();
                }
            }
        }
    }
}