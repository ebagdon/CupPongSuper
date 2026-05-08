using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIDs
{
    public const string IOS_GAME_ID = "4711608";
    public const string ANDROID_GAME_ID = "4711609";
}

public class AdvertisementNames
{
    public const string INTERSTITIAL_IOS_AD_NAME = "Interstitial_iOS";
    public const string INTERSTITIAL_ANDROID_AD_NAME = "Interstitial_Android";

    public const string REWARDED_IOS_AD_NAME = "Rewarded_iOS";
    public const string REWARDED_ANDROID_AD_NAME = "Rewarded_Android";
}

public class Tags
{
    public const string ROOM_TAG = "Room";
    public const string TABLE_TAG = "Table";
    public const string FLOOR_TAG = "Floor";
    public const string CUP_TAG = "Cup";

    public const string BANK_SHOT_BOARD_TAG = "BankShotBoard";

    public const string RAMP_TAG = "Ramp";

    public const string PLAYERS_BALL_TAG = "Player's Ball";

    public const string LANGUAGE_BUTTON_TAG = "LanguageButton";

    public const string RAMP_SOUND_PLAYER_TAG = "RampSoundPlayer";
}

public class MouseAxis
{
    public const string MOUSE_X = "Mouse X";
    public const string MOUSE_Y = "Mouse Y";
}

public class ObjectNames
{
    public const string MainMenuController_NAME = "Main Menu Controller";
    public const string MainMenuRestartSongSoundController_NAME = "MainMenuRestartSongSoundController";

    public const string PLAYERS_BALL_SPAWN_POSITION_NAME = "Player's Ball Spawn Pos";

    public const string BallClickController_Name = "Ball Click Controller";
    public const string BallRespawnManager_NAME = "BallRespawnManager";

    public const string LanguageManager_NAME = "LanguageManager";

    public const string SceneTransitionsManager_NAME = "SceneTransitionsManager";

    public const string ShopInventoryController_NAME = "ShopInventoryController";

    public const string InventorySorter_NAME = "InventorySorter";

    public const string CUP_COLLECTOR_NAME = "Cup Collector";
    public const string CUP_OUTSIDE_NAME = "Outside";
    public const string CUP_INSIDE_NAME = "Inside";
    public const string CUP_RIM_NAME = "Cup rim";
}

public class SceneNames
{
    public const string MAIN_MENU_SCENE_NAME = "MainMenu";

    public const string CLASSIC_GAMEMODE_SCENE_NAME = "ClassicGamemode";
    public const string PARTY_GAMEMODE_SCENE_NAME = "PartyGamemode";
}

public class GamemodeNames
{
    public const string CLASSIC_GAMEMODE_NAME = "CLASSIC";
    public const string PARTY_GAMEMODE_NAME = "PARTY";
}

public class AnimationNames
{
    public const string MAIN_MENU_CUP_IDLE_ANIM_NAME = "Idle";
    public const string MAIN_MENU_CUP_ANIMATION_NAME = "MainMenuCupAnimation";
}

public class UIObjectNames
{
    public const string GAMEMODE_SELECT_CANVAS_NAME = "GamemodeSelectCanvas";
    public const string GAMEOVER_CANVAS_NAME = "GameOverCanvas";

    public const string SETTINGS_CANVAS_NAME = "SettingsCanvas";

    public const string LANGUAGES_SELECT_CANVAS_NAME = "LanguagesSelectCanvas";

    public const string SKINS_CANVAS_NAME = "SkinsCanvas";
    public const string INVENTORY_CANVAS_NAME = "InventoryCanvas";

    public const string PAUSE_CANVAS_NAME = "PauseCanvas";

    public const string SHOP_CANVAS_NAME = "ShopCanvas";
    
    public const string DAILY_REWARDS_CANVAS_NAME = "DailyRewardsCanvas";

    public const string MAIN_MENU_CANVAS_NAME = "MainMenuCanvas";

    public const string BALLS_BUTTON_NAME = "BallsButton";
    public const string CUPS_BUTTON_NAME = "CupsButton";

    public const string YES_BUTTON_NAME = "YesButton";
    public const string NO_BUTTON_NAME = "NoButton";

    public const string BACK_TO_MAIN_MENU_BUTTON_NAME = "BackToMainMenuButton";
    public const string RESTART_BUTTON_NAME = "RestartButton";

    public const string MUSIC_VOLUME_SLIDER_NAME = "MusicVolumeSlider";
    public const string SOUND_EFFECTS_VOLUME_SLIDER_NAME = "SoundEffectsVolumeSlider";

    public const string PING_PONG_BALL_BG_NAME = "PingPongBallBG";

    public const string RED_CUP_BG_NAME = "RedCupBG";
}

public class MixerParameters
{
    public const string MIXER_VOLUME_PARAMETER_NAME = "Volume";
}

public class DataNames
{
    public const string classicGamemodeTutorialDoneData_NAME = "/ClassicGamemodeTutorialDone.dat";
    public const string partyGamemodeTutorialDoneData_NAME = "/PartyGamemodeTutorialDone.dat";

    public const string currentLanguageData_NAME = "/CurrentLanguage.dat";

    public const string masterVolumeData_NAME = "/MasterVolume.dat";

    public const string musicVolumeData_NAME = "/MusicVolume.dat";
    public const string soundEffectsVolumeData_NAME = "/SoundEffectsVolume.dat";

    public const string coinsData_NAME = "/Coins.dat";

    public const string ballThrowMode_LEGACY_DATA_NAME = "/BallThrowMode_LEGACY.dat";
    public const string ballThrowMode_PULLBACK_DATA_NAME = "/BallThrowMode_PULLBACK.dat";

    public const string nextDayToRewardData_NAME = "/NextDayToReward.dat";

    public const string timeLastRewardedData_NAME = "/TimeLastRewarded.dat";
    public const string timeToBeRewardedData_NAME = "/TimeToBeRewarded.dat";

    public const string winsData_NAME = "/Wins.dat";

    public const string timePlayedData_NAME = "/TimePlayed.dat";

    public const string askedToReviewAppData_NAME = "/AskedToReviewApp.dat";

    // VERY IMPORTANT DO NOT CHANGE THIS DATA NAME
    // IT IS USED FOR A IN APP PURCHASE
    public const string playAdsData_NAME = "/PlayAds.dat";
}

public class ShopInventoryDataNames
{

    public const string blue_BALL_OWNED_DATA_NAME = "/blue_BALL_OWNED.dat";
    public const string yellow_BALL_OWNED_DATA_NAME = "/yellow_BALL_OWNED.dat";
    public const string smile_BALL_OWNED_DATA_NAME = "/smile_BALL_OWNED.dat";
    public const string glow_BALL_OWNED_DATA_NAME = "/glow_BALL_OWNED.dat";
    public const string rainbow_BALL_OWNED_DATA_NAME = "/rainbow_BALL_OWNED.dat";
    public const string orangeSplash_BALL_OWNED_DATA_NAME = "/orangeSplash_BALL_OWNED.dat";
    public const string eight_BALL_OWNED_DATA_NAME = "/eight_BALL_OWNED.dat";
    public const string bluePearl_BALL_OWNED_DATA_NAME = "/bluePearl_BALL_OWNED.dat";
    public const string pinkPearl_BALL_OWNED_DATA_NAME = "/pinkPearl_BALL_OWNED.dat";
    public const string lightBulb_BALL_OWNED_DATA_NAME = "/lightBulb_BALL_OWNED.dat";
    public const string crown_BALL_OWNED_DATA_NAME = "/crown_BALL_OWNED.dat";
    public const string diamond_BALL_OWNED_DATA_NAME = "/diamond_BALL_OWNED.dat";
    public const string emerald_BALL_OWNED_DATA_NAME = "/emerald_BALL_OWNED.dat";
    public const string spike_BALL_OWNED_DATA_NAME = "/spike_BALL_OWNED.dat";
    public const string meteor_BALL_OWNED_DATA_NAME = "/meteor_BALL_OWNED.dat";
    public const string hat_BALL_OWNED_DATA_NAME = "/hat_BALL_OWNED.dat";
    public const string belt_BALL_OWNED_DATA_NAME = "/belt_BALL_OWNED.dat";

    public const string redStripe_CUP_OWNED_DATA_NAME = "/redStripe_CUP_OWNED.dat";
    public const string retro_CUP_OWNED_DATA_NAME = "/retro_CUP_OWNED.dat";
    public const string glow_CUP_OWNED_DATA_NAME = "/glow_CUP_OWNED.dat";
    public const string brightStripe_CUP_OWNED_DATA_NAME = "/brightStripe_CUP_OWNED.dat";
    public const string rainbow_CUP_OWNED_DATA_NAME = "/rainbow_CUP_OWNED.dat";
    public const string glowStripe_CUP_OWNED_DATA_NAME = "/glowStripe_CUP_OWNED.dat";

    public const string equipped_BALL_STRING_DATA_NAME = "/equipped_BALL_STRING.dat";

    public const string equipped_CUP_STRING_DATA_NAME = "/equipped_CUP_STRING.dat";

}

public class LootlockerDataNames
{
    public const string playerID_DATA_NAME = "/PlayerID.dat";
    public const string playerName_DATA_NAME = "/PlayerName.dat";
}

public class BallSkinNames
{
    // THESE MUST MATCH WITH THE BALL SKIN STRINGS IN THE INVENTORY

    public const string PING_PONG_BALL_NAME = "PING PONG BALL";
    public const string EIGHT_BALL_NAME = "EIGHT BALL";
    public const string CROWN_BALL_NAME = "CROWN BALL";
    public const string SPIKE_BALL_NAME = "SPIKE BALL";
}

public class CupSkinNames
{
    // THESE MUST MATCH WITH THE CUP SKIN STRINGS IN THE INVENTORY

    public const string RED_CUP_NAME = "RED CUP";
    public const string RED_STRIPE_CUP_NAME = "RED STRIPE CUP";
    public const string RAINBOW_CUP_NAME = "RAINBOW CUP";
}