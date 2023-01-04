
!!!! Attention !!!!
Special offer for the moment: Write a review for AntiCheat Free and write a mail to guardingpearsoftware@gmail.com and win AntiCheat Pro.
!!!! Attention !!!!

Introduction
-------------------

Cheating has been around since the beginning of time. Whether it is athletes using performance-enhancing drugs, 
students cheating on exams, or politicians lying to the public, people have always found ways to get ahead dishonestly.

Also in video games cheating is unfortunately common, by using various methods to take advantage oneself over other players. 
This can include techniques such as cheating through the use of mods, hacks, or exploits; or simply playing the 
game in an unintended way that gives an unfair advantage. It can be performed on single-player or multiplayer games. 
In multiplayer games, cheating can involve either manipulating the game itself to gain an advantage 
or playing with other players who are cheating.

To prevent cheating / hacking or manipulating your game data and memory, GuardingPearSoftwares AntiCheat was developed.


AntiCheat Free features
-------------------

+ Variable protection: Encrypts your variables/fields to prevent memory manipulation.

+ PlayerPrefs protection: Encrypts the Unity PlayerPrefs to secure your data and settings against manipulation.


Variable protection
-------------------
Most of your data, for example positions or health, is stored in the runtime memory.
This memory is easy accessible by cheat tools or data sniffer.
To prevent unwanted modification of the data in the memory, you have to encrypt it.
To do so, you can use AntiCheats protected fields, to protect your application runtime data
against cheater.

Using the protected fields is straightforward:
1) Create a GameObject and attach the OPS.AntiCheat.Detector.FieldCheatDetector Component.

2) Include the namespace OPS.AntiCheat.Field in your scripts - Here you find all the protected field types.

3) Replace your unprotected field types with the protected one.
Example: int to ProtectedInt32

4) To get a callback if a cheater got detected, attach to the OPS.AntiCheat.Detector.FieldCheatDetector.Singleton.OnFieldCheatDetected event.

5) More examples can be found in the Protected_Fields demo.


PlayerPrefs protection
-------------------
Unitys PlayerPrefs are useful to store user settings and user data.
Unfortunately these are not protected or encrypted and can be easily modified.
To prevent this, you can use the AntiCheat Protected PlayerPrefs.

Using the protected player prefs is straightforward:
1) Include the namespace: OPS.AntiCheat.Prefs

2) Here you find 2 classes.
-> ProtectedPlayerPrefs: Replaces the default functions of the Unity PlayerPrefs and adds some new. The protected prefs will be stored at the know default location.

-> ProtectedFileBasedPlayerPrefs: Is a custom implementation of the Unity PlayerPrefs allowing to store the player prefs protected at a custom file path.
   To assign a custom file path, set ProtectedFileBasedPlayerPrefs.FilePath. Now use ProtectedFileBasedPlayerPrefs like you would use the default Unity PlayerPrefs.
   
3) For some more examples, have a look at the Protected_PlayerPrefs demo.