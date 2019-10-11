namespace FluidCdk.IAM.Grants
{
    public class RekognitionGrant : GrantBase
    {
        
        public RekognitionGrant FullAccess()
        {
            _ = Actions("rekognition:*");
            return this;
        }


    }
}
