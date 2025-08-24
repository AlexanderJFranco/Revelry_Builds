-- This function will be called by C# when the player interacts

object = {
    x = 100,
    y = 100,
    atlas = "images/dev_assets/t1_atlas.xml",
    sprite = "vendor1-animation",
    hitbox_color = "#0000FF19",  
    debug_enabled = true;
    physics_type = "Solid"
}

dialogue = {
    start = {
        speaker = "Cal",
        text = "Where am I?",
        next = "guardian_intro"
    },

    guardian_intro = {
        speaker = "Guardian",
        text = "You have entered the ancient halls...",
        choices = {
            { text = "Who are you?", next = "guardian_identity" },
            { text = "I should leave.", next = "end" }
        }
    },

    guardian_identity = {
        speaker = "Guardian",
        text = "I am bound to this spear.",
        next = "end"
    },

    ["end"] = {
        speaker = "Cal",
        text = "..."
    }
}
function onInteract()
    printMessage("Interaction Successful")
end