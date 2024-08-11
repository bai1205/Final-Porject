import os

import numpy as np

# 读取之前保存的.npy文件
#oaded_color_labels = np.load('instance_labels/instance_label_19.npy')
loaded_color_labels = np.load('language_labels/apple_labels_0.npy')
# 假设 array_with_labels 是一个包含XYZ坐标和颜色标签的数组，这里用上面的示例

# 取出最后一列
color_labels = loaded_color_labels[:, -1]

# 对最后一列进行修改，除了0以外的所有数字都改为1
color_labels[color_labels != 0] = 1

# 将修改后的列赋值回原数组
loaded_color_labels[:, -1] = color_labels

print(loaded_color_labels)
unique_numbers2 = set(np.unique(loaded_color_labels[:, -1]))
# 打印不同数字的数量和它们本身
#print("实例这些数字包括：", unique_numbers)
print("语义这些数字包括：", unique_numbers2)
