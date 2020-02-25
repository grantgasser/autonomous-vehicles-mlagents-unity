import os
import matplotlib.pyplot as plt
from PIL import Image
import numpy as np
import cv2
import tensorflow as tf
from tensorflow.keras import datasets, layers, models
from sklearn.model_selection import train_test_split

DATA_PATH = 'recording_data/'
IMG_EXTENSION = '.png'
SEED = 42
TEST_SIZE = 0.2

def read_image_data():
    """
    Reads image data from folder using OpenCV.

    Args:

    Returns:
        images (List[np.array]): each element is an image or np array of size (256,256,3)
    """
    images = []
    for i, file in enumerate(os.listdir(DATA_PATH)):
        if file.endswith(IMG_EXTENSION):
            # NOTE: cv2 uses BGR, not RGB
            img = cv2.imread(os.path.join(DATA_PATH, file), 1)
            images.append(img)

    return np.array(images)

def viz_image(np_image):
    """
    Visualizes provided image

    Args:
        np_image (np.array): image of size (256, 256, 3)
    """

    # reverse to RGB to visualize
    img = cv2.cvtColor(np_image, cv2.COLOR_BGR2RGB)
    plt.imshow(img)
    plt.show()


def main():
    # read and viz
    images = read_image_data()
    viz_image(images[np.random.randint(0, len(images))])

    # train/test split
    # random labels for now
    labels = np.random.rand(len(images), 1)

    x_train, x_test, y_train, y_test = train_test_split(images, labels, test_size=TEST_SIZE, random_state=SEED)

    # normalize between 0-1
    x_train, x_test = x_train / 255.0, x_test / 255.0

    # create model
    # ---------------
    # output size = W (input) - K (filter) + 1 = 256 - 3 + 1 = 254
    model = models.Sequential()
    model.add(layers.Conv2D(x_train.shape[1], (3,3), activation='relu', input_shape=(x_train.shape[1:])))
    model.add(layers.MaxPooling2D((2, 2)))
    model.add(layers.Conv2D(64, (3, 3), activation='relu'))
    model.add(layers.MaxPooling2D((2, 2)))
    model.add(layers.Flatten())
    model.add(layers.Dense(64, activation='relu'))
    model.add(layers.Dense(1))

    # review model
    print(model.summary())
    # ---------------

    # compile train model
    # ---------------
    # model.compile(optimizer='adam',
    #               loss=tf.keras.losses.MeanSquaredError,
    #               metrics=['MeanSquaredError'])
    #
    # history = model.fit(x_train, y_train, epochs=5, validation_data=(x_test, y_test))
    # ---------------

    # evaluate model
    # ---------------
    # plt.plot(history.history['accuracy'], label='accuracy')
    # plt.plot(history.history['val_accuracy'], label='val_accuracy')
    # plt.xlabel('Epoch')
    # plt.ylabel('Accuracy')
    # plt.ylim([0.5, 1])
    # plt.legend(loc='lower right')
    #
    # test_loss, test_acc = model.evaluate(x_test, y_test, verbose=2)
    # print('\nTest Accuracy:', test_acc)

    # ---------------



main()