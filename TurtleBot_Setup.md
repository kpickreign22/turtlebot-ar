# TurtleBot Control Instructions

This README details the steps to establish a connection with a TurtleBot and execute various ROS commands both on the TurtleBot and the host computer.

## Prerequisites
- SSH access to the TurtleBot
- ROS Kinetic installed on both the TurtleBot and Noetic on the host computer
- Pre-configured network settings on both devices

## Setup Instructions

### Connect to TurtleBot via SSH
1. Open a terminal on your host computer.
2. Execute the SSH command to connect to the TurtleBot:
   ```bash
   ssh turtlebot@[TB_IP]
### Set Environment Variables on TurtleBot
3. Change ros IP:
   ```bash
   export ROS_IP = [TB_IP]
### Start the TurtleBot
4. Launch the minimal setup for TurtleBot:
   ```bash
   roslaunch turtlebot_bringup minimal.launch --screen
### Teleoperation via Keyboard
5. Run the keyboard teleoperation node to control the TurtleBot manually:
   ```bash
   roslaunch turtlebot_teleop keyboard_teleop.launch
### Set Up on Host Computer
Configure ROS Master and Local IPs
### Set up the environment variables on the host computer:
   ```bash
   export ROS_MASTER_URI=http://[TB_IP]:11311
   export ROS_IP=[LAPTOP_IP]
   ```
### Launch RViz on Host Computer
Start RViz to visualize the robot's state:
   ```bash
   rosrun rviz rviz
   ```
### Start Gmapping and Navigation on TurtleBot
Launch the gmapping demo on the TurtleBot:
   ```bash
   roslaunch turtlebot_navigation gmapping_demo.launch
   ```
### For navigation with a specific map (e.g., 4th flor JCC):
   ```bash
   export TURTLEBOT_MAP_FILE=~/4th_floor_combined.yaml
   roslaunch turtlebot_navigation amcl_demo.launch
   ```
### Start the WebSocket Server on TurtleBot
Launch the WebSocket server to communicate with external applications:
   ```bash
   roslaunch rosbridge_server rosbridge_websocket.launch
   ```
## Troubleshooting
Ensure that all IP addresses and environment variables are correctly set.
Check network connectivity if you cannot SSH into the TurtleBot or if ROS nodes cannot communicate.



