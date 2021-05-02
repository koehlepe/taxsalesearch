import numpy as np
import matplotlib
import matplotlib.pyplot as plt

# Adapted from example at https://matplotlib.org/stable/gallery/images_contours_and_fields/image_annotated_heatmap.html

comments_weights = range(1,5)
title_weights = range(1,5)

#Results from trec_eval
maps = np.array([[0.4549, 0.4582, 0.4656, 0.4678],
                    [0.4552, 0.4603, 0.4678, 0.4712],
                    [0.4582, 0.4678, 0.5005, 0.5002],
                    [0.4678, 0.5002, 0.5117, 0.5080],])

fig, ax = plt.subplots()
im = ax.imshow(maps)

# We want to show all ticks...
ax.set_xticks(np.arange(len(title_weights)))
ax.set_yticks(np.arange(len(comments_weights)))
# ... and label them with the respective list entries
ax.set_xticklabels(title_weights)
ax.set_yticklabels(reversed(comments_weights))

ax.set_xlabel("Title Weight")
ax.set_ylabel("Comments Weight")

# Rotate the tick labels and set their alignment.
plt.setp(ax.get_xticklabels(), rotation=45, ha="right",
         rotation_mode="anchor")

# Loop over data dimensions and create text annotations.
for i in range(len(comments_weights)):
    for j in range(len(title_weights)):
        text = ax.text(j, i, maps[i, j],
                       ha="center", va="center", color="w")

ax.set_title("MAP With Different Field Weights")
fig.tight_layout()
plt.show()