# AACR-Lexical-Tools
C# app to calculate balance of scores and lexical diversity measurements on datasets of responses.

AACR Lexical Tools was developed to support the AACR research group’s needs to examine how balanced are scores assigned to categories, in addition to exploring if lexical diversity measurements may inform the development of machine learning models of student responses. 
It was programmed in C# by Marisol Mercado Santiago, a member of the AACR research group, CREATE for STEM Institute, Michigan State University.

It stems words from a set of student responses input into the app, and calculates lexical diversity measurements from those stemmed words. 
Lexical analysis/diversity measurements that calculates: Type-to-token ratio (TTR), Brunet's Index, Guiraud's R, Honore's Statistic, Herdan's C, Carroll's Corrected TTR, Somers' S, Dugast's Uber Index, Yule's K, and vocd-D[1].

In addition, it calculates a measurement of balance of scores[2] from a set of scores input into the app.

Please, refer to this documentation for more information on how it works: https://github.com/BeyondMultipleChoice/AACR-Lexical-Tools/blob/master/Documentation%20of%20AACR%20Lexical%20Tools%20-%20Google%20Docs.pdf

The code is open to others to run, contribute, or learn from it. 


Note: We are not responsible for the software outputs or accuracy of the calculations.

Acknowledgements: 
This material is based upon work supported by the National Science Foundation (Grants 1323162 and 1347740). 
Any opinions, findings and conclusions or recommendations expressed in this material are those of the author(s) and do not necessarily reflect the views of the supporting agencies.

------------------------------------------------------------
[1] Vocd-d reference:
McKee, Malvern, & Richards. (2000). Measuring vocabulary diversity using dedicated software. Literary and Linguistic Computing, 15(3), 323–338. https://doi.org/10.1093/llc/15.3.323

[2] Reference: https://stats.stackexchange.com/questions/239973/a-general-measure-of-data-set-imbalance
