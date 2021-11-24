if(isObject(lob2_bot_menu)) {
	lob2_bot_menu.delete();
}

function lob2_bot_menu_show() { 
	canvas.pushDialog(lob2_bot_menu);
}

function lob2_bot_menu_hide() { 
	canvas.popDialog(lob2_bot_menu);
}

//--- OBJECT WRITE BEGIN ---
new GuiControl(lob2_bot_menu) {
   profile = "GuiDefaultProfile";
   horizSizing = "right";
   vertSizing = "bottom";
   position = "0 0";
   extent = "640 480";
   minExtent = "8 8";
   enabled = "1";
   visible = "1";
   clipToParent = "1";

   new GuiSwatchCtrl() {
      profile = "GuiDefaultProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "121 60";
      extent = "321 168";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      color = "155 155 155 220";

      new GuiTextEditCtrl(lob2_bot_menu_name) {
         profile = "GuiTextEditProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "76 60";
         extent = "176 18";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         maxLength = "255";
         historySize = "0";
         password = "0";
         tabComplete = "0";
         sinkAllKeyEvents = "0";
      };
      new GuiMLTextCtrl() {
         profile = "GuiMLTextProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "75 26";
         extent = "183 20";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         lineSpacing = "2";
         allowColorChars = "0";
         maxChars = "-1";
         text = "<just:center><font:a:23px>Bot Name";
         maxBitmapHeight = "-1";
         selectable = "0";
         autoResize = "0";
      };
      new GuiButtonCtrl() {
         profile = "GuiButtonProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "99 98";
         extent = "128 35";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         command = "lob2_bot_menu_ok();";
         text = "Okay";
         groupNum = "-1";
         buttonType = "PushButton";
      };
   };
};
//--- OBJECT WRITE END ---

function lob2_bot_menu_ok() {
	%name = lob2_bot_menu_name.getValue();
	talk(%name);
}
