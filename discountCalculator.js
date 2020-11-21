'use strict';

const BOOK_PRICE = 8.00;
const quantityDiscount = [
    { count: 2, percent: 0.05 },
    { count: 3, percent: 0.10 },
    { count: 4, percent: 0.20 },
    { count: 5, percent: 0.25 }
];

export default class DiscountCalculator {
    // Calculate total price for all books in the cart sans discounts
    getTotal(cart) {
        return cart.length * BOOK_PRICE;
    }

    // Calculate best discount possible for all scenarios
    getBestDiscount(cart) {
        let groups = this.getGroups(cart);

        //console.log("Before rebalance: " + this.getCombinedDiscount(groups));
        //this.displayGroups(groups);

        let maxGroupSize = Math.max(...groups.map(o => o.length), 0);
        let pairs = this.getRebalancePairs(maxGroupSize);
        this.rebalance(groups, pairs);

        let discount = this.getCombinedDiscount(groups);

        return { discount: discount, discountGroups: groups };
    }

    // calculate total discount for multiple groups of books
    getCombinedDiscount(groups) {
        let discountAmount = 0;

        for (const group of groups) {
            const discount = quantityDiscount.find(x => x.count == group.length);
            if (discount) {
                const total = this.getTotal(group);
                discountAmount += Number((total * discount.percent).toFixed(2));
            }
        }

        return discountAmount;
    }

    // split cart into multiple groups ensuring that each group does not have
    // duplicates, and that each group is as large as possible (aka greedy)
    getGroups(cart) {
        let groups = [];

        let cartCopy = [...cart];
        while (cartCopy.length > 0) {
            let unique = [];
            for (let i = cartCopy.length - 1; i >= 0; --i) {
                if (unique.indexOf(cartCopy[i]) < 0) {
                    unique.push(cartCopy[i]);
                    cartCopy.splice(i, 1);
                }
            }
            groups.push(unique);
        }

        return groups;
    }

    // Return an array of pairs of group sizes where rebalancing between two
    // groups of those sizes would improve the overall discount.  For example,
    // if the return value is [ { large: 5, small: 3 } ], then any time we can
    // find a group of 5 and a group of 3, we should move one book from the
    // larger group to the smaller group.
    getRebalancePairs(maxGroupSize) {
        let pairs = [];
    
        // total savings possible per group size
        let discountAmount = new Array(maxGroupSize);
        for (let i = 0; i < maxGroupSize; ++i) {
            const numBooks = i + 1;
            const discount = quantityDiscount.find(x => x.count == numBooks);
            const discountPercent = (discount) ? discount.percent : 0;
            discountAmount[i] = (numBooks * BOOK_PRICE) * discountPercent;
        }
    
        // find all possible combinations where moving one book between groups
        // would improve the total savings
        for (let large = maxGroupSize - 1; large > 0; --large) {
            for (let small = 0; small < large - 1; ++small) {
                const before = discountAmount[small] + discountAmount[large];
                const after = discountAmount[small + 1] + discountAmount[large - 1];
                if (after > before) {
                    const gain = after - before;
                    pairs.push({ large: large + 1, small: small + 1, gain: gain });
                }
            }
        }
    
        // sort pairs by amount gained (descending)
        pairs.sort((a, b) => {
            return b.gain - a.gain;
        });

        return pairs;
    }
    
    rebalance(groups, pairs) {
        for (let pair of pairs) {
            let done = false;
            while (!done) {
                let large = groups.find(x => x.length == pair.large);
                let small = groups.find(x => x.length == pair.small);
                if (large && small) {
                    this.moveOneBook(large, small);
                } else {
                    done = true;
                }
            }
        }
    }

    moveOneBook(fromGroup, toGroup) {
        for (let i = 0; i < fromGroup.length; ++i) {
            const book = fromGroup[i];
            if (toGroup.indexOf(book) < 0) {
                toGroup.push(book);
                fromGroup.splice(i, 1);
                return true;
            }
        }

        return false;
    }

    displayGroups(groups) {
        for (const group of groups) {
            const bookNumbers = group.sort((a, b) => {
                return a - b;
            });
            console.log("Len(" + group.length + "): " + bookNumbers.join(", "));
        }
    }
}
