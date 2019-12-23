# ShouldBeLike

[Shouldly](https://github.com/shouldly/shouldly) like tool for asserting likeness based on [James Fosters' DeepEqual library](https://github.com/jamesfoster/DeepEqual).

Examples:

	new MyObject().ShouldBeLike(new MyObject()) // passes

	new MyObject() { Property = "actually something" }.ShouldBeLike(new MyObject() { Property = "Something expected" } ) // fails