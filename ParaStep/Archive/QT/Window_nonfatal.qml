import QtQuick 2.9
import QtQuick.Layouts 1.3
import QtQuick.Controls 2.3
import QtQuick.Controls.Material 2.1

ApplicationWindow{
id: window
    height: 480
    width: 640
    visible: true
    color: "#262626"
    
    Shortcut {
            sequences: ["Esc", "Back"]
            enabled: stackView.depth > 1
            onActivated: {
                stackView.pop()
                listView.currentIndex = -1
            }
        }
    
        Shortcut {
            sequence: "Menu"
            onActivated: optionsMenu.open()
        }
    
    
    
    Drawer {
            id: drawer
            width: Math.min(window.width, window.height) / 3 * 2
            height: window.height
            interactive: stackView.depth === 1
    
            ListView {
                id: listView
    
                focus: true
                currentIndex: -1
                anchors.fill: parent
    
                delegate: ItemDelegate {
                    width: parent.width
                    text: model.title
                    highlighted: ListView.isCurrentItem
                    onClicked: {
                        listView.currentIndex = index
                        stackView.push(model.source)
                        drawer.close()
                    }
                }
    
                model: ListModel {
                    ListElement { title: "Main"; source: "Test_nonfatal.qml" }
                }
    
                ScrollIndicator.vertical: ScrollIndicator { }
            }
        }
    StackView {
            id: stackView
            anchors.fill: parent
    
            initialItem: "Test_nonfatal.qml"
        }
}
