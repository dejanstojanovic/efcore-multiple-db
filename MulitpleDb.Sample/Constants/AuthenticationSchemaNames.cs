namespace MulitpleDb.Sample.Constants
{
    public static class AuthenticationSchemaNames
    {
        public const string ApiKeyAuthentication = nameof(ApiKeyAuthentication);
        public const string BasicAuthentication = nameof(BasicAuthentication);

        public const string MixSchemaNames = nameof(ApiKeyAuthentication)+","+ nameof(BasicAuthentication);
    }
}
