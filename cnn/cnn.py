import os
import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
#import cv2
import tensorflow as tf
from tensorflow.keras import layers, models
from sklearn.model_selection import train_test_split
import datetime
import constants

def read_image_data():
    """
    Reads image data from folder using OpenCV.

    Args:

    Returns:
        images (np.array): each element is an image or np array of size (256,256,3)
        labels (np.array): numpy array of labels (angle columns from csv)
    """
    images = []
    labels = []
    for i, file in enumerate(os.listdir(constants.DATA_PATH)):
        if file.endswith(constants.IMG_EXTENSION) and file[9] != '1':  # skip 1st dark data set
            # NOTE: cv2 uses BGR, not RGB
            img = imread(os.path.join(constants.DATA_PATH, file), 1)
            images.append(img)
        elif file.endswith('.csv') and file[9] != '1':
            labs = pd.read_csv(os.path.join(constants.DATA_PATH, file))
            labels.extend(labs.wheel_angle)

    return np.array(images), np.array(labels)

def viz_image(np_image, label):
    """
    Visualizes provided image

    Args:
        np_image (np.array): image of size (256, 256, 3)
        label (float): label for given image
    """

    # reverse to RGB to visualize
    img = cv2.cvtColor(np_image, cv2.COLOR_BGR2RGB)
    plt.imshow(img)
    plt.title('Example Input Image w/ label=' + str(label))
    plt.savefig(constants.OUTPUT_DATA_PATH + 'example_image')
    plt.savefig(constants.CURRENT_STATE_PATH + 'example_image')
    plt.show()


def main():
    # read data
    print('Reading data..')
    images, labels = read_image_data()
    assert len(images) == len(labels), 'Input and Label sizes have to be same!'
    print('Finished reading data.')

    # for output data
    if not os.path.exists(constants.OUTPUT_DATA_PATH):
        os.makedirs(constants.OUTPUT_DATA_PATH)

    # visualize data
    idx = np.random.randint(0, len(images))
    viz_image(images[idx], labels[idx])

    # train/test split
    print('Train/test split')
    x_train, x_test, y_train, y_test = train_test_split(images,
                                                        labels,
                                                        test_size=constants.TEST_SIZE,
                                                        random_state=constants.SEED
                                                        )
    # write test data for later prediction
    np.save(os.path.join(constants.OUTPUT_DATA_PATH, 'x_test.npy'), x_test)
    np.save(os.path.join(constants.OUTPUT_DATA_PATH, 'y_test.npy'), y_test)

    # normalize between 0-1
    x_train, x_test = x_train / 255.0, x_test / 255.0

    # create model
    # ---------------
    # output size = W (input) - K (filter) + 1 = 256 - 3 + 1 = 254
    print('Constructing model..')
    model = models.Sequential()
    model.add(layers.Conv2D(x_train.shape[1], (3, 3), activation='relu', input_shape=(x_train.shape[1:])))
    model.add(layers.MaxPooling2D((2, 2)))
    model.add(layers.Conv2D(64, (3, 3), activation='relu'))
    model.add(layers.MaxPooling2D((2, 2)))
    model.add(layers.Flatten())
    model.add(layers.Dense(64, activation='relu'))
    model.add(layers.Dense(1))

    # review model
    print(model.summary())
    # ---------------

    # compile train model (NOTE: loss is MSE)
    # ---------------
    model.compile(optimizer='adam', loss=tf.keras.losses.MSE)

    # tensorboard
    # log_dir = "logs/fit/" + datetime.datetime.now().strftime("%Y%m%d-%H%M%S")
    # tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir=log_dir, histogram_freq=1)

    history = model.fit(x_train, y_train,
                        epochs=5, validation_data=(x_test, y_test),
                        #callbacks=[tensorboard_callback]
                        )
    # ---------------

    # evaluate model
    # ---------------
    plt.plot(history.history['loss'], label='Train MSE/Loss')
    plt.plot(history.history['val_loss'], label='Val MSE/Loss')
    plt.xlabel('Epoch')
    plt.ylabel('MSE/Loss')
    plt.legend(loc='upper right')
    plt.savefig(constants.OUTPUT_DATA_PATH + 'loss_plot')
    plt.savefig(constants.CURRENT_STATE_PATH + 'loss_plot')
    plt.show()

    test_loss = model.evaluate(x_test, y_test, verbose=2)
    print('\nTest Loss:', test_loss)
    # ---------------

    # save model
    # ---------------
    # reset metrics before saving
    model.reset_metrics()

    model.save(os.path.join(constants.OUTPUT_DATA_PATH, 'cnn_v1.h5'))
    # ---------------


main()