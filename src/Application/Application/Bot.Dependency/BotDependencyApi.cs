using System;
using GGroupp.Infra;
using PrimeFuncPack;

namespace GGroupp.Internal.Support.Bot;

using IADUserGetFunc = IAsyncValueFunc<ADUserGetIn, Result<ADUserGetOut, Failure<Unit>>>;
using IUserGetFunc = IAsyncValueFunc<UserGetIn, Result<UserGetOut, Failure<UserGetFailureCode>>>;
using IIncidentCreateFunc = IAsyncValueFunc<IncidentCreateIn, Result<IncidentCreateOut, Failure<IncidentCreateFailureCode>>>;
using ICustomerSetFind = IAsyncValueFunc<CustomerSetFindIn, Result<CustomerSetFindOut, Failure<CustomerSetFindFailureCode>>>;

internal static class BotDependencyApi
{
    public static Dependency<IUserGetFunc> UseUserGetApi()
        =>
        UseDataverseApiClient("UserGetApi").UseUserGetApi();

    public static Dependency<IIncidentCreateFunc> UseIncidentCreateApi()
        =>
        UseDataverseApiClient("IncidentCreateApi").UseIncidentCreateApi();

    public static Dependency<ICustomerSetFind> UseCustomerSetSearchApi()
        =>
        UseDataverseApiClient("CustomerSetFindApi").UseCustomerSetSearchApi();

    public static Dependency<IADUserGetFunc> UseADUserGetApi()
        =>
        UseStandardHttpMessageHandler("ADUserGetApi")
        .With(
            BotServiceProvider.GetConfiguration<ADUserApiClientConfiguration>)
        .UseADUserGetApi();

    private static Dependency<IDataverseApiClient> UseDataverseApiClient(string loggerCategoryName)
        =>
        UseStandardHttpMessageHandler(loggerCategoryName)
        .With(
            BotServiceProvider.GetConfiguration<DataverseApiClientConfiguration>)
        .UseDataverseApiClient();

    private static Dependency<LoggerDelegatingHandler> UseStandardHttpMessageHandler(string loggerCategoryName)
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging(
            sp => sp.GetLogger(loggerCategoryName));
}