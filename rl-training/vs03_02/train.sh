#!/bin/bash
mlagents-learn config/vs03_config.yaml --env=vs03 --run-id=$1 --num-envs=$2 --train --no-graphics
