namespace ParaStep.QtErrorHandler.QML_Types
{
    public class NetObjectsModel
    {
        public NetObject GetNetObject()
        {
            return new NetObject();
        } 
        
        public class NetObject
        {
            public void UpdateProperty(string value)
            {
                Property = value;
            }
            
            public string Property { get; set; }
        }
    }
}