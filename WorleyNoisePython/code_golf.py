import random as R,PIL.Image as I
a=R.randrange
r=range
s=400
p=tuple((a(0,s),a(0,s))for _ in r(50))
i=I.new('L',(s,s))
i.putdata([255-min((d:=x-t[0],e:=y-t[1],d*d+e*e)[-1]for t in p)//28 for y in r(s)for x in r(s)])
i.save('w.png')