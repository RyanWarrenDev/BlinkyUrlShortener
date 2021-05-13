# BlinkyUrlShortener
Payroc - Url shortener

# Payroc audition!

Hi Andrew, apologies for the delay in getting this to you. I've actually been through this exact scenario of URL shortening before so I knew it was never going to be as simple as it looks at face value.

I know in the spec it said to use frameworks etc I was familiar with but I actually took this as an opportunity to try something newish. So I went with Blazor and LiteDb neither of which I've used before but given my C# background they ended up being pretty straightforward. 

For the short URL generation I've used a basic random number generator and Base64 (URL safe characters) encoded the results. I've used a Cryptographic random number generator to help try make the results truly random as the CRNG has a more reliable distribution of results compared to the standard Random.

Although because it's random collisions can occur. I've included a test to check for collisions but because it's random it's luck of the draw as to whether it works or not really. I was able at one point to 9Mil keys without a collision but like I said, just pure luck.

Although I realise this probably isn't what you were looking for. I have been pressed for time as of late and given the 48hr window this is what I went with. There are additional checks to ensure tokens aren't duplicated but this involves checking the database. 

## Reality

In reality this would have been a distributed microservice with a distributed systems manager such as Apache Zookeeper. I'm happy to go over this scenario with you in a call if you'd like. In reality on application start-up it would connect to ZooKeeper and be assigned an integer range. 
This counter would then be used as the basis for the key generation, being Base64 encoded. This would ensure no two machines have the same range assigned and prevent the need for a database hit to check for collisions. It's also likely you would use a distributed cache and not an in memory cache with the the cache eviction policy of LRU to remove stagnant URLs and keep the most popular in the cache.

Yes if the instance were to go down you would lose any remaining keys (using persistent keys and not ephemeral, important!) in the assigned range but given the number of possibilities it isn't too large of a concern.

## Apologies
Again apologies for both the delay and also not implementing this as a microservice architecture which I suspect is what you were looking for. 
Thanks you for your time and if this is the end for me I still appreciate the opportunity, it was fun to do something different :)