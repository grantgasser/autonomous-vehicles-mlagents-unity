# Docker Setup

## Overview
For training, we have set up a docker containiner that can be deployed to a server. Unity cannot be easily ran remotly because it requires a license, but a unity execuitable can be built and ran in headless mode for training the rl model.

## Docker Helpers
* Dockerfile
	- Based on the python docker image base
	- Installs require python libraries and copy latest RL and CNN codebase
* `./docker_build.sh x.x`
	* params:
		- version (e.g. 0.1)
	* builds the docker image
* `./docker_run.sh x.x`
	* params:
		- version (e.g. 0.1)
	* run the docker image (bash)
	* NOTE: This will not grant access to the gpu. Need CUDA support

## How to train
### RL
* run the docker image
* `cd rl-training`
* `./train.sh $0 $1`
	- $0 = runid (e.g. test01)
	- $1 = number of environment (e.g. 1)
		- Only run more environments if using the GPU (we trainined with 6)

### CNN
* `cd cnn`
* Train the CNN: `./train.sh`
* Make predictions: `./predict.py`
