'use strict';

import DiscountCalculator from "./discountCalculator.js";

const Book1 = 1;
const Book2 = 2;
const Book3 = 3;
const Book4 = 4;
const Book5 = 5;

function addToCart(cart, bookNumber, quantity) {
    while (quantity-- > 0) {
        cart.push(bookNumber);
    }
}

let cart = [];
addToCart(cart, Book1, 2);
addToCart(cart, Book2, 2);
addToCart(cart, Book3, 2);
addToCart(cart, Book4, 1);
addToCart(cart, Book5, 1);

const calc = new DiscountCalculator();
const { discount, discountGroups } = calc.getBestDiscount(cart);

const total = calc.getTotal(cart);
console.log("Total: ", total.toFixed(2));
console.log("Discount: ", discount.toFixed(2));
console.log("Net: ", (total - discount).toFixed(2));

console.log("\nGroupings:");
calc.displayGroups(discountGroups);
