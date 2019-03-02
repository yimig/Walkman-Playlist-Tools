import requests
import re
from bs4 import BeautifulSoup
 
 
 
headers={
        'Host':'music.163.com',
        'Referer':'http://music.163.com/',
 
        'User-Agent':'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36'
        }
 
 
 
url='http://music.163.com/discover/toplist'
r=requests.session()
r=BeautifulSoup(r.get(url,headers=headers).content)
result=r.find('ul',{'class':'f-hide'}).find_all('a')
#print(reslut)
 
music=[]  #用于接受返回值
for mu in result:
    #print('{}:{}'.format(music.text,music['href']))
    c='{}:{}'.format(mu.text,mu['href'])
    music.append(c)
 
print(music)
music[1]
from pprint import pprint  #格式化输出
pprint(music)

