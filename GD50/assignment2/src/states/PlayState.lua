--[[
    GD50
    Breakout Remake

    -- PlayState Class --

    Author: Colton Ogden
    cogden@cs50.harvard.edu

    Represents the state of the game in which we are actively playing;
    player should control the paddle, with the ball actively bouncing between
    the bricks, walls, and the paddle. If the ball goes below the paddle, then
    the player should lose one point of health and be taken either to the Game
    Over screen if at 0 health or the Serve screen otherwise.
]]

PlayState = Class{__includes = BaseState}

--[[
    We initialize what's in our PlayState via a state table that we pass between
    states as we go from playing to serving.
]]
function PlayState:enter(params)
    self.paddle = params.paddle
    self.bricks = params.bricks
    self.health = params.health
    self.score = params.score
    self.highScores = params.highScores
    self.ball = {}
    self.ball[1] = params.ball
    self.level = params.level

    self.powerups = {}
    self.numPowerups = 0
    self.ballsInPlay = 1
    self.key = params.key
    self.recoverPoints = 5000

    -- give ball random starting velocity
    self.ball[self.ballsInPlay].dx = math.random(-200, 200)
    self.ball[self.ballsInPlay].dy = math.random(-50, -60)
end

function PlayState:update(dt)
    if self.paused then
        if love.keyboard.wasPressed('space') then
            self.paused = false
            gSounds['pause']:play()
        else
            return
        end
    elseif love.keyboard.wasPressed('space') then
        self.paused = true
        gSounds['pause']:play()
        return
    end

    -- update positions based on velocity
    self.paddle:update(dt)
    for k, balls in pairs(self.ball) do
        balls:update(dt)
    end

    for k, power in pairs(self.powerups) do
        power:update(dt)
    end

    for k, balls in pairs(self.ball) do
        if balls:collides(self.paddle) then
            -- raise ball above paddle in case it goes below it, then reverse dy
            balls.y = self.paddle.y - 8
            balls.dy = -balls.dy

            --
            -- tweak angle of bounce based on where it hits the paddle
            --

            -- if we hit the paddle on its left side while moving left...
            if balls.x < self.paddle.x + (self.paddle.width / 2) and self.paddle.dx < 0 then
                balls.dx = -50 + -(8 * (self.paddle.x + self.paddle.width / 2 - balls.x))
            
            -- else if we hit the paddle on its right side while moving right...
            elseif balls.x > self.paddle.x + (self.paddle.width / 2) and self.paddle.dx > 0 then
                balls.dx = 50 + (8 * math.abs(self.paddle.x + self.paddle.width / 2 - balls.x))
            end

            gSounds['paddle-hit']:play()
        end
    end

    -- detect collision across all bricks with the ball
    for k, brick in pairs(self.bricks) do

        for k, balls in pairs(self.ball) do

            -- only check collision if we're in play
            if brick.inPlay and balls:collides(brick) then
                -- check if brick is locked
                if brick.locked == true and self.key == false then
                    -- if hitting locked brick then give higher chance of it dropping key powerup
                    if math.random(10) == 1 and table.getn(self.powerups) < 4 and self.key == false then
                        self.numPowerups = self.numPowerups + 1
                        self.powerups[self.numPowerups] = Powerup(2, brick.x + 16, brick.y + 8)
                    end
                else
                    -- add to score
                    self.score = self.score + (brick.tier * 200 + brick.color * 25)

                    if math.random(30) == 1 and table.getn(self.powerups) < 4 then

                        self.numPowerups = self.numPowerups + 1
                        self.powerups[self.numPowerups] = Powerup(1, brick.x + 16, brick.y + 8)

                    elseif math.random(20) == 5 and table.getn(self.powerups) < 4 and self.key == false then

                        self.numPowerups = self.numPowerups + 1
                        self.powerups[self.numPowerups] = Powerup(2, brick.x + 16, brick.y + 8)

                    end

                    -- trigger the brick's hit function, which removes it from play
                    brick:hit()

                    -- if we have enough points, recover a point of health
                    if self.score > self.recoverPoints then

                        -- cant go above 4 paddle size max
                        self.paddle.size = math.min(4, self.paddle.size + 1)
                        self.paddle.width = math.min(128, self.paddle.width + 32)

                        -- can't go above 3 health
                        self.health = math.min(3, self.health + 1)

                        -- multiply recover points by 2
                        self.recoverPoints = math.min(10000000, self.recoverPoints * 2)

                        -- play recover sound effect
                        gSounds['recover']:play()
                    end

                    -- go to our victory screen if there are no more bricks left
                    if self:checkVictory() then
                        gSounds['victory']:play()
                        self.paddle.size =  2
                        self.paddle.width = 64

                        gStateMachine:change('victory', {
                            level = self.level,
                            paddle = self.paddle,
                            health = self.health,
                            score = self.score,
                            highScores = self.highScores,
                            ball = balls,
                            recoverPoints = self.recoverPoints
                        })
                    end
                end

                --
                -- collision code for bricks
                --
                -- we check to see if the opposite side of our velocity is outside of the brick;
                -- if it is, we trigger a collision on that side. else we're within the X + width of
                -- the brick and should check to see if the top or bottom edge is outside of the brick,
                -- colliding on the top or bottom accordingly 
                --

                -- left edge; only check if we're moving right, and offset the check by a couple of pixels
                -- so that flush corner hits register as Y flips, not X flips
                if balls.x + 2 < brick.x and balls.dx > 0 then
                    
                    -- flip x velocity and reset position outside of brick
                    balls.dx = -balls.dx
                    balls.x = brick.x - 8
                
                -- right edge; only check if we're moving left, , and offset the check by a couple of pixels
                -- so that flush corner hits register as Y flips, not X flips
                elseif balls.x + 6 > brick.x + brick.width and balls.dx < 0 then
                    
                    -- flip x velocity and reset position outside of brick
                    balls.dx = -balls.dx
                    balls.x = brick.x + 32
                
                -- top edge if no X collisions, always check
                elseif balls.y < brick.y then
                    
                    -- flip y velocity and reset position outside of brick
                    balls.dy = -balls.dy
                    balls.y = brick.y - 8
                
                -- bottom edge if no X collisions or top collision, last possibility
                else
                    
                    -- flip y velocity and reset position outside of brick
                    balls.dy = -balls.dy
                    balls.y = brick.y + 16
                end

                -- slightly scale the y velocity to speed up the game, capping at +- 150
                if math.abs(balls.dy) < 150 then
                    balls.dy = balls.dy * 1.02
                end

                -- only allow colliding with one brick, for corners
                break
            end
        end
    end

    -- checks powerups in play for collision then adds more balls into play when powerup is hit 
    for k, power in pairs(self.powerups) do
        if power:collides(self.paddle) then
            if power.skin == 1 then
                table.remove(self.powerups, k)
                self.numPowerups = self.numPowerups - 1
                gSounds['recover']:play()
                if self.ballsInPlay < 15 then
                    for i = 0, 1 do
                        self.ballsInPlay = self.ballsInPlay + 1
                        self.ball[self.ballsInPlay] = Ball(math.random(7))
                        self.ball[self.ballsInPlay].x = self.paddle.x + (self.paddle.width / 2) - 4
                        self.ball[self.ballsInPlay].y = self.paddle.y - 8
                        self.ball[self.ballsInPlay].dx = math.random(-200, 200)
                        self.ball[self.ballsInPlay].dy = math.random(-50, -60)
                    end
                end
            -- checks if key powerup is hit and sets key to true if so
            elseif power.skin == 2 then
                self.key = true
                self.numPowerups = self.numPowerups - 1
                table.remove(self.powerups, k)
                gSounds['recover']:play()
            end
        end
    end

    -- if ball goes below bounds, revert to serve state and decrease health if it was the last ball
    for k, balls in pairs(self.ball) do
    
        if balls.y >= VIRTUAL_HEIGHT and self.ballsInPlay == 1 then
            self.paddle.size = self.paddle.size - 1
            self.paddle.width = math.max(32, self.paddle.width - 32)
            self.health = self.health - 1
            gSounds['hurt']:play()

            if self.health == 0 then
                gStateMachine:change('game-over', {
                    score = self.score,
                    highScores = self.highScores
                })
            else
                gStateMachine:change('serve', {
                    paddle = self.paddle,
                    bricks = self.bricks,
                    health = self.health,
                    score = self.score,
                    highScores = self.highScores,
                    level = self.level,
                    recoverPoints = self.recoverPoints,
                    key = self.key
                })
            end
        elseif balls.y >= VIRTUAL_HEIGHT and self.ballsInPlay > 1 then
            table.remove(self.ball, k)
            self.ballsInPlay = self.ballsInPlay - 1
            gSounds['hurt']:play()

        end
    end

    -- for rendering particle systems
    for k, brick in pairs(self.bricks) do
        brick:update(dt)
    end

    if love.keyboard.wasPressed('escape') then
        love.event.quit()
    end
end

function PlayState:render()
    -- render bricks
    for k, brick in pairs(self.bricks) do
        brick:render()
    end

    -- render all particle systems
    for k, brick in pairs(self.bricks) do
        brick:renderParticles()
    end

    for k, power in pairs(self.powerups) do
        power:render()
    end
    self.paddle:render()

    for k, balls in pairs(self.ball) do
        balls:render()
    end

    renderScore(self.score)
    renderHealth(self.health)

    -- pause text, if paused
    if self.paused then
        love.graphics.setFont(gFonts['large'])
        love.graphics.printf("PAUSED", 0, VIRTUAL_HEIGHT / 2 - 16, VIRTUAL_WIDTH, 'center')
    end
end

function PlayState:checkVictory()
    for k, brick in pairs(self.bricks) do
        if brick.inPlay then
            return false
        end 
    end

    return true
end