import os
import matplotlib.pyplot as plt
from PIL import Image


DATA_PATH = 'recording_data/'
IMG_EXTENSION = '.png'

def main():
    # load training data
    train = []
    for i, file in enumerate(os.listdir(DATA_PATH)):
        if file.endswith(IMG_EXTENSION):
            print(file)

            img = Image.open(os.path.join(DATA_PATH, file))

            # look at one file
            if i == 0:
                img.show()

            train.append(img)

    print('Train size:', len(train))

main()