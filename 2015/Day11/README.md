## --- Day 11: Corporate Policy ---
Santa's previous password expired, and he needs help choosing a new one.

To help him remember his new password after the old one expires, Santa has devised a method of coming up with a password based on the previous one.  Corporate policy dictates that passwords must be exactly eight lowercase letters (for security reasons), so he finds his new password by <em>incrementing</em> his old password string repeatedly until it is valid.

Incrementing is just like counting with numbers: <code>xx</code>, <code>xy</code>, <code>xz</code>, <code>ya</code>, <code>yb</code>, and so on. Increase the rightmost letter one step; if it was <code>z</code>, it wraps around to <code>a</code>, and repeat with the next letter to the left until one doesn't wrap around.

Read the [full puzzle](https://adventofcode.com/2015/day/11).
