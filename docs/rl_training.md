# Reinforcement Training
Our reinforcement learning model is trained using the built in Proximal Policy Optimization from ML-Agents. Our latest hyper parameters can be found in `config/config.yaml` 

## Debugging Environment
We debugged our model to tweak our initial hyper parameters using a smaller testing environment. We moved the start and end boxes to simulate, first, a straight road, then a curve.

## RL Training Run 01
* name: ``
* training command: ``
* configuration file: `...`
* behavioral hyperparameter: 
  * **Constant Torque**: `50`
  * **Decision Interval**: `4`
  * **Max Step**: `0` (unlimted)
  * **Reset on Done**: `true`
  * **Vector Observation Space**: `5`
  * **Vector Action Space**: `1`
  * **Number of Concurrent Agents**: 12
* Machine Details:
  * Ran From Unity
  * GTX 1080
  * Locally (personal desktop Ubuntu 18.4)

### Results
* **Summary Directory**: `...`
* **NN File**: `...`