# Autonomous Driving in Unity using ML Agents 

* [Development Documentation](docs/README.md)
* [Setup Video](https://youtu.be/Ike1bEuBuNI)
* Unity Apps for best trained RL Model:
  - [Mac](https://blainerothrock-public.s3.us-east-2.amazonaws.com/autonomous-vehicle-sim/submission/mac_road003_vs_03_02.zip)
  - [Linux](https://blainerothrock-public.s3.us-east-2.amazonaws.com/autonomous-vehicle-sim/submission/linux_road003_vs03_02.zip)
  
## Docker
[Docker docs](docs/docker.md)

## Project Overview
A simulation model using Unity and MLAgents to explore a solution for lane keeping in autonomous vehicles. The purpose of this project is to explore deep learning in simulation environment and explore Unity's ML-Agent toolkit. We took a 2 part approach in exploring this problem:

* **Part 1**: Build a environment using Unity to train a vehicle agent using reinforcement learning.
  * This model will be guided using oracle knowledge of the world.
  * This is intended to save time with the ability to generate driving data.
* **Part 2**: Using the RL modeled trainined in part 1, training a CNN model for lane keeping using only data internal to the vehicle.
  * This is to simulate a more realistic model

See Detailed [Slide Deck](docs/presentation_deck.pdf) for more info

## Overview of Reinforcement Learning Results
![vs03_02_gif](docs/images/vs03/vs03_02_full_track_8x.gif)
8x playback

For more details see the following: 
* Trained model apps:
  * [Mac](https://blainerothrock-public.s3.us-east-2.amazonaws.com/autonomous-vehicle-sim/submission/mac_road003_vs_03_02.zip)
  * [Linux](https://blainerothrock-public.s3.us-east-2.amazonaws.com/autonomous-vehicle-sim/submission/linux_road003_vs03_02.zip)
* [RL Training](docs/rl_training.md)
* [Simulation Environment](docs/simulation_environment.md)
* [Unity](docs/Unity.md)
* [Vehicle Agent](docs/vehicle_agent.md)

### Reinforcement Learning I/O
See more details in [Vehicle Agent.](docs/vehicle_agent.md)
#### Observations
* Wheel Angle, normalized `[-1, 1]`
* Front Distance to Center, normalized `[-1, 1]`
* Back Distance to Center, normalized `[-1, 1]`
* Z axes velocity from the vehicle's rigidbody
* X axes velocity from the vehicle's rigidbody

#### Actions
 * **0**: no change in wheel angle
 * **1**: add `.25` degrees (turn right)
 * **2**: add `-.25` degrees (turn left)

## Overview of CNN Results
The CNN did not give us our desired results in the given amount of time. We decided not to include the running of our CNN in our docker image because it does not provide satisfactory results. For more details see [CNN Training](docs/convolutional_net.md).

### Inspiration
* [ML Agents repo](https://github.com/Unity-Technologies/ml-agents)
* [ML Agents paper](https://arxiv.org/pdf/1809.02627.pdf)
* Build a driving environment in Unity, then use the ML Agents toolkit to train our car (agent) to drive within lanes using reinforcement learning

### Authors
* [Grant Gasser](https://www.linkedin.com/in/grantgasser/)
* [Blaine Rothrock](https://www.linkedin.com/in/brothrock/)

