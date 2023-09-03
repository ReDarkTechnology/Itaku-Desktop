function OnSourceChanged(url)
  Browser.ExecuteFile("main.js")
end

function OnNavigationStarting(url)
  Browser.ExecuteFile("main.js")
end

function OnNavigationCompleted(url)
  Browser.ExecuteFile("main.js")
end

function OnDOMContentLoaded()
  Browser.ExecuteFile("main.js")
end
