# Autonomous Driving in Unity using ML Agents 
## Advanced Deep Learning Project

* [Development Documentation](docs/README.md)

### Authors
* [Grant Gasser](https://www.linkedin.com/in/grantgasser/)
* [Blaine Rothrock](https://www.linkedin.com/in/brothrock/)

## V1 Results
![vs01_gif](docs/images/vs01/vs01.gif) 
![vs01_reward](docs/images/vs01/reward_log.png) 
![vs01_loss](docs/images/vs01/loss_log.png) 

### Thoughts on V1 and Next Steps
* Lane-keeping is fairly good, want to discretize actions to limit "jerkiness"
* Train on AWS using pre-configured [Deep Learning AMI](https://aws.amazon.com/marketplace/pp/B077GCH38C) 
* Test on different tracks
* Move on to Part 2 (see **Project Plan** towards bottom)

### Unity Environment + ML Agents
* With our self-built driving environment in Unity, we use the ML Agents toolkit to train our car to drive within lanes using reinforcement learning
* [ML Agents repo](https://github.com/Unity-Technologies/ml-agents)
* [ML Agents paper](https://arxiv.org/pdf/1809.02627.pdf)

## Project Plan
* Part 1: lane keeping with RL agent, after trained, collect images (1-8 cameras) with associated wheel angle and acceleration
* Part 2: Conv Net in TensorFlow, connect to Unity environment with [Python API](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md)
  - Input: Images collected from RL agent and (our driving)
  - Targets: (wheel angle, torque, ...)
* Stretch goal: Use lidar (or radar) and add several RL agents 

## Packaging and Running 
[AWS Deep Learning AMI](https://aws.amazon.com/getting-started/tutorials/train-deep-learning-model-aws-ec2-containers/)
