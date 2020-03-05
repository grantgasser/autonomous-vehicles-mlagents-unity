import tensorflow as tf
import os
import constants
import numpy as np

# load model
saved_model = tf.keras.models.load_model(os.path.join(constants.OUTPUT_DATA_PATH, 'cnn_v1.h5'))

# load test data
x_test = np.load(os.path.join(constants.OUTPUT_DATA_PATH, 'x_test.npy'))
y_test = np.load(os.path.join(constants.OUTPUT_DATA_PATH, 'y_test.npy'))

# make predictions
new_predictions = saved_model.predict(x_test)

for i, pred in enumerate(new_predictions):
    print('pred:', pred, 'y_test', y_test[i])

#np.testing.assert_allclose(predictions, new_predictions, rtol=1e-6, atol=1e-6)