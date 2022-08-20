# How to use:
- Clone the Repo
- Build the App
- A Message should popup that settings must be entered first.
- Goto Directory "...\ProxerMeToMyAnimeList\ProxerMeToMyAnimeList\bin\Debug" or "Release"
- Open File "ProxerMeToMyAnimeList.exe.config"
- Fill the 3 Settings
  - "MAL_CLIENT_ID" - Check section "MyAnimeList API"
  - "MAL_CLIENT_SECRET" - Check section "MyAnimeList API"
  - "PROXER_ANIMELIST_URL" - Check section "Proxer Anime List"
- Start the App
 - The App will fetch any animes as completed (Todo to determine the current state)
 - Just follow the Messages in the App


# MyAnimeList API
This App uses the MyAnimeList API to fetch data und update entrys to the users anime list
- You need a MyAnimeList Account
- Get API Access on this site "https://myanimelist.net/apiconfig"
- Enter your API Information in the config

# Proxer Anime List
The Proxer API is not public, that the reason we will scrap all important informations from the site.
1 downsite is, that we need to complete many captchas to fetch the Anime information needed.
- Check if your Anime List is set to public
- Copy the URL to your public Anime List into the Config

# Known problems:
- Manual captcha solving needed (got no API access)
- Sync to MAL only work ~70%. Proxer.me Anime names (Original, Jap. Eng.) dont match with MAL
- All Animes detected on your Proxer.me Anime List will be marked as "completed"

# Todo:
- Creating a "good" Readme.md
- Refactoring
- Add a check to determine which Animes on Proxer are completed, canceled or on the Todo list
