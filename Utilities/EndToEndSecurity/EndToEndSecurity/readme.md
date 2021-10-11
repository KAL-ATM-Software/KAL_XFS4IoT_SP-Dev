# Requirements

## Basic framework

### Error handling
The framework will support clean handling of programatic errors. Typically this should mean restarting the firmware. 

### Logging
THe framework will support logging information for debugging. 

## Basic functionality
- Valid a token 
The framework will support validating if a token is correct, and returning a simple result plus extra information
- Report on key values in the last token
The framework will support reporting the value of known keys in a token. 

## Nonce handling
There are two strategies for generating a nonce (non-repeating number, used in token security). 
    not reset to zero on power cycles. This means that the hardware must have persistent storage to track the last nonce value. 
    - A simple incrementing integer can be used - 1,2,3 etc. However, the integer _must_ continue to integrate across restarts. It must 
    - A strong random number can be used. In practice this is best if there is a hardware random number generator. The same rule applies, 
    that the number must not repeat across power cycles, so using a psudo-random number would require tracking the seed persistently 
    across restarts, in which case it would be simpler just to use a persistent integer. 

Note that the framework doesn't include support for nonce handling directly. The hardware must either support hardware random number generation or persistent tracking. 

- Get new nonce
- Clear current nonce
- Compare to current nonce

# Extension points
The framework is supplied as source for a static link library with unresolved external references. To use the framework, these 
references should be implemented in whatever way is required for the specific implementation. 