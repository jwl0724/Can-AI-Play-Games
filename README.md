# Can-AI-Play-Games
A neural network to see how good AI can get at playing a simple game. If you would like to play the simple game directly you can download the game here: https://drive.google.com/file/d/1BwbQNVippF8s6juWKaFZ3OhgXXFCHCnp/view

The neural net was created by pastra98 and can be found here https://github.com/pastra98/NEAT_for_Godot

# How to Run
1. Download Godot 4.3 and import this project on the Godot Launcher
2. Press the play button in the editor to launch the training scene and run it

# How to Adjust Training Parameters
Training parameters are parameters involved in the process of training that dictate the training environment

1. In the editor file explorer, open Scenes/Levels/TrainingScene.tscn

2. After opening click on the root node of the scene (TrainingScene)
3. At the top of the inspector under the TrainingManager section are the variables which you can change
4. Once your variables are set, press the play button and the new variables will be used

**Variables**

*Network Name* - The name of the network file to be saved once training is done, the file can be found at C:\Users\YourName\AppData\Roaming\Godot\app_userdata\YourProject\network_configs

*Training Loop* - Number of training iterations to go through

*Reward Amount* - The amount of seconds added to the time limit when a new best is found in a generation

*Fitness Threshold* - The acceptance criteria of training at which training will stop once met

*Net Config Name* - The name of the network config file that can be found at C:\Users\YourName\AppData\Roaming\Godot\app_userdata\YourProject\param_configs

**NOTE: IF THIS DIRECTORY DOES NOT EXIST, RUN THE PROJECT FIRST TO CREATE THE DIRECTORY AND IT WILL CREATE A DEFAULT FILE**

*Agent Count* - The number of agents to create in training, reduce this number if lag is encountered

# How to Adjust Network Parameters
Network parameters are parameters involved in network configurations that dictate network functionality

1. Go to C:\Users\YourName\AppData\Roaming\Godot\app_userdata\YourProject\param_configs in file explorer
    - **Note: if this path does not exist run the project first to create the directory**

2. Copy Default.cfg in the same directory and rename it to whatever you want
3. Adjust the values in the copy to whatever configuration you like and save your changes
4. Go back to the Godot editor and change the *Net Config Name* parameter **(see above)** to the name of the copy that you made in step 3 **WITHOUT THE FILE EXTENSION**
5. Run the project after changing the *Net Config Name* parameter
    - **Note: An error will be thrown if the file does not exist, the project will enter a breakpoint and stay open even when the window is closed, if you encounter this press the stop (Square icon) on the top right of the editor to close the game**
