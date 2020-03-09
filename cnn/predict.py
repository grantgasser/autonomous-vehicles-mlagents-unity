import tensorflow as tf
import os
import constants
import numpy as np

# load model
saved_model = tf.keras.models.load_model(os.path.join(constants.OUTPUT_DATA_PATH, 'cnn_v1.h5'))

# load test data
x_test = np.load(os.path.join(constants.OUTPUT_DATA_PATH, 'x_test.npy'))
y_test = np.load(os.path.join(constants.OUTPUT_DATA_PATH, 'y_test.npy'))

x_test = np.float32(x_test)

# make predictions
new_predictions = saved_model.predict(x_test)

assert(len(new_predictions) == len(y_test))

print('\nViewing some predictions:')
for i, pred in enumerate(new_predictions):
    print('pred:', pred, 'y_test:', y_test[i])
    if i == 10:
        break