## Hard Shell Studios Input Manager V2 - By Haydn Comley ##
// Contact me at haydncomley@gmail.com or haydn@haydncomley.com for any help or information you may need.

[Document Stucture]
	1. Intro
	2. Basics + All Functions with Descriptions.
	3. Rebinding Techniques
	4. Basic XML Layout

## INTRODUCTION ##

// Welcome and thank you for downloading the manager. In this document you'll find a basic overview of how to get started and what the different parts of the manager can do.
// If you prefer this in a video form then some tutorials can be found on our youtube channel at: https://www.youtube.com/edit?o=U&video_id=KlbgHXN9iKQ


// All of the inputs are stored in a XML file. The default inputs are saved within a Unity Resource file. This will be created if one is not currently present.
// The default file is created within "Assets/Resources/InputManager/" and called DefaultBindings.xml.
// This resource file can be placed anywhere within the project, but has to be structued like "<some-directory-in-your-project>/Resources/InputManager".

// These are then saved in an XML file for players the file Unity specifies in Application.persistantDataPath. On windows this is "AppData/LocalLow/CompanyName/GameName/KeyBindings.xml"

// All inputs can be edited from the Unity Inspector by going to the titlebar and pressed "Window" then "Edit Inputs" (Roughly the 4th item down).
// As well as this you can also open the XML file and edit that directly. This might be easier to do so if adding lots of inputs at the same time etc.

## END OF INTRODUCTION ##


## BASICS + ALL FUNCTIONS ##

// All basic Input functions have to be preceded with "hardInput." just as the unity ones would be "Input."
// For example you would replace "Input.GetKey(Key.W)" or "Input.GetButton("Forward")" to "hardInput.GetKey("Forward")".
	## I understand Unity uses "GetButton" for named bindings and I've chosen to use "GetKey". If this is a problem for you, or you want to rename them then open the file "hardInput.cs" and rename the functions as desired.

[Functions + Unity default]

	UNITY							/	HARD INPUT MANAGER										/	DESCRIPTION
	
	Input.GetButton("Forward")		-	hardInput.GetKey("Forward")								-	Returns true when the key is held down.
	
	Input.GetButtonDown("Forward")	-	hardInput.GetKeyDown("Forward")							-	Returns true once when the key is held down.
	
	Input.GetButtonUp("Forward")	-	hardInput.GetKeyUp("Forward")							-	Returns true once when the key is released after being pressed.
		
	Input.GetAxis("Forward")		-	hardInput.GetAxis("Forward")							-	Returns a float from 0 to 1. 0 = Not pressed, when holding the key down this ramps up to 1. (3 is the default for sensitivity as it is in Unity)	
																												// To make this ramp quicker (i.e. to have snappier / quicker inputs then increase the key's sensitivity)
	
	Input.GetAxis("Vertical")		-	hardInput.GetAxis("Forward", "Backward")				-	Unlike Unity, this manager only has "Positive" keys, it doesn't have "Negatives". In Unity a negative key ramps from 0 to -1. And the positive 0 to 1.
																																													  If both are being held this number goes to 0.
																									This function allows you to pass two keys in. A positive and a negative, e.g. Forward and Backward, or Right and Left.
	
	N/A								-	hardInput.SetKey("KeyName", InputType, WantPrimary)		-	This sets the input for a key. KeyName is the name of the bind you want to set. E.g. "Forward". InputType can be either a KeyCode, KeyAxis or
																																																	KeyController.
																									This is mainly used to rebind keys and can be found inside the ButtonRebind.cs script if you want to learn more.
																									I'd reccomend not touching this and leaving rebinding up to the UI_ButtonRebind script unless you know what you're doing.
																									WantPrimary needs to be set to true if you want the set the primary, or false for the secondary.
	
	N/A								-	hardInput.Sensitivity("KeyName")						-	This returns the sensitivity of the key passed.
	
	N/A								-	hardInput.Sensitivity("KeyName", Sensitivity)			-	This sets the sensitiviity to the value passed to the key wanted.
	
	Input.anyKey					-	hardInput.AnyKey()										-	Returns true if any key is pressed down.

	N/A								-	hardInput.AnyKeyKEY()									-	Returns the current KeyCode of the key pressed. Returns KeyCode.none if nothing is being pressed.

	N/A								-	hardInput.AnyKeyCONTROLLER()							-	Returns the current KeyController of the controller key pressed. Returns KeyController.none if nothing is being pressed on a controller.

	N/A								-	hardInput.GetKeyName("KeyName", WantPrimary)			-	Returns a string of a formatted key name. This returns a better looking keyname. E.g.	Instead of "Alpha1" it returns "1".
																																															Instead of "Mouse0" it returns "Left Mouse".
																									WantPrimary needs to be set to true if you want the set the primary, or false for the secondary.

	N/A								-	hardInput.FormatText("String")							-	Returns a formatted string with the keybindings filled in.
																									// This is to be used in order to have chunks of text formatted so that you can place the actual binding names in text.
																									For example your text might say "Press W to walk forward". But if a player rebinds the forward key then this text will be inaccurate.
																									So if you used hardInput.FormatText("Press ['Forward',P] to walk forward.") this would return "Press <key> to walk forward."
																																																<key> would be the input for that button.
																														The syntax for using this is square brackets []
																														That include the keyname quoted (' <- that, not -> ")
																														E.g. ['Forward']
																														And then this needs to be followed by either a P or an S. If its a P then it returns the Primary Key, and S = Secondary
																														E.g. ['Forward',P] might be 'W' and ['Forward',S] might be 'Up Arrow' if these are your bindings.

	N/A								-	hardInput.GetAllBindings()								-	Returns an array of all the current bindings and there attributes.

	N/A								-	hardInput.ResetKey("KeyName")							-	Resets the specified key to its default value.

	N/A								-	hardInput.ResetAll()									-	Resets all bindings to their default values.

	N/A								-	hardInput.MousePositionWorld()							-	Returns a Vector3 of the mouse in the world position. Basically short-hand for Camera.main.ScreenPointToWorld(Input.mousePosition).

	N/A								-	hardInput.MousePositionWorld2D()						-	Same as above but replaces the Z value with 0.


## END OF BASICS ##


## REBINDING TECHNIQUES ##

	//	If you want to rebind keys for users then i reccomend using the included "ButtonRebind.cs" script as it has all the functionality needed.
		This can be simply dragged onto an existing UI Button.

		1.	To set it up. Simply type in the KeyName that you want to be the target.
		2.	If you want it to be for the secondary input then check the "Use Secondary" box, if not leave it empty.
		3.	The "AutoText" check-box is provided to help simplfy your bindings. If this is checked, then it will ignore the current text in the specified
			Text element and fill it with either the KeyName or "Press a key" when being bound. (This text can be changed in the script)
			If left unticked then it will simply just format the Text element specified. This means if you want to display keys in a different way than the
			default by having it checked you can.


	// You can also always rebind via script with hardInput.SetKey()

## END OF REBINDING TECHNIQUES ##




## XML BASIC LAYOUT ##

<KeyBindings> // These are opening and closing tags that the manager looks for. //

	<Input Name="String" Primary="Type.Code" Secondary="Type.Code" Sensitivity="Float" />

	// Each key is required to have a Name, Primary, Secondary and Sensitivity value assigned. //

</KeyBindings>

## BASIC LAYOUT END ##