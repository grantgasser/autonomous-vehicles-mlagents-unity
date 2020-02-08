# Reinforcement Training
Our reinforcement learning model is trained using the built in Proximal Policy Optimization from ML-Agents. Our latest hyper parameters can be found in `config/config.yaml` 

## Debugging Environment
We debugged our model to tweak our initial hyper parameters using a smaller testing environment. We moved the start and end boxes to simulate, first, a straight road, then a curve.

## RL Training Run 01
* name: `sv01`
* training command: `mlagents-learn config/sv01_config.yaml --run-id=sv01 --train`
* configuration file: `config/sv01_config.yaml`
* behavioral hyperparameter: 
  * **Steps**: `5 million` force stopped `~1.2 million`
  * **Constant Torque**: `50`
  * **Decision Interval**: `4`
  * **Max Step**: `0` (unlimted)
  * **Reset on Done**: `true`
  * **Vector Observation Space**: `5`
  * **Vector Action Space**: `1`
  * **Number of Concurrent Agents**: `12`
* Machine Details:
  * Ran From Unity
  * GTX 1080
  * Locally (personal desktop Ubuntu 18.4)

### Results
* **Summary Directory**: `rl-training/summaries/vs01_VehicleBrain`
* **NN File**: `rl-training/models/vs01-0/vs01-VehicleBrain.nn`


![vs01_gif](images/vs01/vs01.gif) 
![vs01_reward](images/vs01/reward_log.png) 
![vs01_loss](images/vs01/loss_log.png) 

### Findings
The vehicle generally learned the track converging at about 500K steps. At about 900K the model stated to decrease in reward and episode length. While the vehicle was able to stay in the lane, we do not believe this is a good model to generate data from. The biggest problem, we believe, is the continuous action. The action is given in the form of a -1, 1 value which is then translated to a -30, 30 scale for the wheel angle. This is a infinite space that vastly under values the action to continue with the current angle. If you watch the video, the car is jerky and the wheel angle changes drastically from step to step. Ideally we would want a smooth angle transition. In some cases we can see the wheel angles toggling from -30 to 30, which is not realistic. In the simulation the time between steps does not allow for the wheel to turn it's entire degree range and therefor results in a jerky straight line.  

### Next Steps
Redesign the action to be a classification with the following classes:
* positive wheel angle delta
* negative wheel angle delta
* constant wheel angle

This will only allow the wheels to move *k* degree(s) in one direction in a step and have a even representation for no change in direction. Hyperparameters such as decision interval and the vector observation space will need to be adjusted. May consider a small reward for keeping the direction constant.