import numpy as np
import matplotlib.pyplot as plt
import os

getPath = os.getcwd()

resolutionFile = open(getPath + "\\..\\..\\Resolution.txt")
resolutionString = resolutionFile.read()
resolutionValues = resolutionString.split(",")
width = int(resolutionValues[0])
height = int(resolutionValues[1])

textFile = open(getPath + "\\..\\..\\Render.txt")
giantString = textFile.read()
values = giantString.split(",")
values.pop()

finalArray = []

timesToIterate = height * width * 3

for i in range(timesToIterate):
    finalArray.append(float(values[i]))

finalArray = np.reshape(finalArray, (height, width, 3))


plt.imsave(getPath + "\\..\\Scene_Render\\Render.png", finalArray)

resolutionFile.close()
textFile.close()
