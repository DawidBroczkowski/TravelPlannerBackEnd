namespace TravelPlanner.Domain.Models
{
    [Flags]
    public enum Permission : long
    {
        // Basic User Permissions
        None = 0,
        CreateAccount = 1L << 0,
        CreateTravelPlan = 1L << 1,
        SaveTravelPlan = 1L << 2,
        ViewTravelPlan = 1L << 3,
        EditTravelPlan = 1L << 4,
        DeleteTravelPlan = 1L << 5,

        // Public Travel Plan Permissions
        MakeTravelPlanPublic = 1L << 6,
        MakeTravelPlanPrivate = 1L << 7,
        ViewPublicTravelPlans = 1L << 8,
        CommentOnPublicTravelPlan = 1L << 9,
        EditOwnCommentOnPublicPlan = 1L << 10,
        DeleteOwnCommentOnPublicPlan = 1L << 11,

        // Content Interaction Permissions
        CommentOnAttraction = 1L << 12,
        EditOwnComment = 1L << 13,
        DeleteOwnComment = 1L << 14,

        // Advanced User and Community Contributions
        CreateAttraction = 1L << 15,
        EditAttraction = 1L << 16,
        DeleteAttraction = 1L << 17,

        // Moderator Permissions
        ModerateComments = 1L << 18,
        ModerateAttractions = 1L << 19,
        ModerateUsers = 1L << 20,
        ViewFlaggedContent = 1L << 21,
        ModerateCommentsOnPublicPlans = 1L << 22,
        ModeratePublicTravelPlans = 1L << 23,
        AccessPrivateContent = 1L << 24,

        // Administrative Permissions
        ManageUsers = 1L << 25,
        ManageRoles = 1L << 26,
        AccessAnalytics = 1L << 27,
        ManageSiteSettings = 1L << 28,
        ViewAdminReports = 1L << 29,
        AccessAdminPanel = 1L << 30,
        FeaturePublicTravelPlan = 1L << 31,
        FeatureAttraction = 1L << 32,

        // SuperAdmin Permissions
        All = long.MaxValue
    }

    public enum PenaltyType
    {
        None,
        Warning,
        Mute,
        AccountBan
    }

    public enum EntityType
    {
        None,
        User,
        TravelPlan,
        Attraction,
        Comment,
        UserProfile
    }

    public enum MediaType
    {
        None,
        Images,
        Videos,
        Documents,
        Others
    }

    public enum Voivodship
    {
        None,
        Dolnośląskie,
        KujawskoPomorskie,
        Lubelskie,
        Lubuskie,
        Łódzkie,
        Małopolskie,
        Mazowieckie,
        Opolskie,
        Podkarpackie,
        Podlaskie,
        Pomorskie,
        Śląskie,
        Świętokrzyskie,
        WarmińskoMazurskie,
        Wielkopolskie,
        Zachodniopomorskie
    }
}
