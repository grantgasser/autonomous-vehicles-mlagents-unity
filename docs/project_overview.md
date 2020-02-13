## Project Plan
* Part 1: lane keeping with RL agent, after trained, collect images (1-8 cameras) with associated wheel angle and acceleration
* Part 2: Conv Net in TensorFlow, connect to Unity environment with [Python API](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md)
  - Input: Images collected from RL agent and (our driving)
  - Targets: (wheel angle, torque, ...)
* Stretch goal: Use lidar (or radar) and add several RL agents 
