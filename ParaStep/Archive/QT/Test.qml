import QtQuick 2.7
import QtQuick.Controls 2.0
import QtQuick.Layouts 1.0
import QtQuick.Controls.Material 2.1
import net 1.1
Rectangle{
    id: base
    visible: true
    color: "#262626"
    border.color: "#00abff"
    activeFocusOnTab: true
    focus: true

    Button {
        visible: true
        id: okay
        x: base.width - 108
        y: base.height - 38
        width: 100
        height: 30
        text: qsTr("Oh ok")
        highlighted: false
        wheelEnabled: false
        autoExclusive: false
        checkable: false
        flat: false
        onClicked: {
            var exit = netexit.getNetExit()
            exit.run()
        }
    }

    NetExceptionModel{
        id: excmodel
    }
    NetExitHandler{
        id: netexit
    }

    Text {
        id: header
        x: 8
        y: 8
        width: base.width - 8
        height: 43
        color: "#ffffff"
        text: qsTr("ParaStep has encountered a fatal error, and needs to exit")
        font.pixelSize: 22
        horizontalAlignment: Text.AlignHCenter
        verticalAlignment: Text.AlignVCenter
        font.family: "Tahoma"
    }

    Rectangle {
        id: stackTraceBorder
        x: 8
        y: 108
        width: base.width - 16
        height: base.height - 152
        color: "#353535"
        border.color: "#00abff"

        Text {
            id: stackTrace
            x: 8
            y: 8
            width: base.width - 16
            height: base.height - 160
            color: "#ffffff"
            text: qsTr("ass")
            
            font.pixelSize: 12
            //cursorVisible: false
            //readOnly: true
            
            Component.onCompleted: {
                var ex = excmodel.getNetException()
                stackTrace.text = ex.stackTrace
            }
        }
    }


    Rectangle {
        id: exceptionTypeBorder
        x: 8
        y: 57
        width: base.width - 16
        height: 45
        color: "#353535"
        border.color: "#00abff"

        TextInput {
            id: exceptionType
            x: 8
            y: 8
            width: base.width - 32
            height: 29
            color: "#ffffff"
            text: qsTr("exception type")
            font.pixelSize: 14
            horizontalAlignment: Text.AlignHCenter
            verticalAlignment: Text.AlignVCenter
            readOnly: true
            cursorVisible: false
            
            Component.onCompleted: {
                var ex = excmodel.getNetException()
                exceptionType.text = ex.header
            }
        }
    }
    
}






/*##^##
Designer {
    D{i:0;autoSize:true;height:480;width:640}D{i:6}
}
##^##*/
